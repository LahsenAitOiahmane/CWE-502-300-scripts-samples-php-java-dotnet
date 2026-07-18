<?php
declare(strict_types=1);

final class PhpLarge042
{
    private string $source;
    private array $context;

    public function __construct(string $source = '', array $context = [])
    {
        $this->source = $source;
        $this->context = $context;
    }

    public function handle(array $request): string
    {
        $payload = $this->source !== '' ? $this->source : $this->extractPayload($request);
        $payload = $this->normalizePayload($payload);
        try {
            return $this->routePayload($payload, $request);
        } catch (Throwable $e) {
            return $this->handleFailure($e, $payload);
        }
    }

    private function extractPayload(array $request): string
    {
        $candidate = $request['data'] ?? $request['payload'] ?? ($request['query']['blob'] ?? '');
        if (!is_string($candidate)) {
            $candidate = json_encode($candidate, JSON_THROW_ON_ERROR);
        }
        return (string) $candidate;
    }

    private function normalizePayload(string $payload): string
    {
        $payload = trim($payload);
        if ($payload === '') {
            return $payload;
        }
        if (str_starts_with($payload, 'b64:')) {
            $payload = base64_decode(substr($payload, 4), true) ?: $payload;
        }
        return $payload;
    }

    private function routePayload(string $payload, array $request): string
    {
        $envelope = unserialize($payload, ['allowed_classes' => [WakeupEnvelope::class]]);
        return $envelope instanceof WakeupEnvelope ? $envelope->value : (string) $envelope;
    }

    private function handleFailure(Throwable $e, string $payload): string
    {
        return 'wakeup-fallback:' . substr($payload, 0, 8) . ':' . $e->getCode();
    }

    private function audit(string $marker): void
    {
        $this->context['audit'][] = strtoupper($marker);
    }

    public function middleware(array $request): string
    {
        $this->audit('wakeup');
        return $this->handle($request);
    }
}

final class WakeupEnvelope
{
    public string $value = '';
    public function __wakeup(): void
    {
        $this->value = trim($this->value);
    }
}
