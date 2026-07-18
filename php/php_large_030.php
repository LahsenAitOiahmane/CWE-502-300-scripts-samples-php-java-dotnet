<?php
declare(strict_types=1);

final class PhpLarge030
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
        $path = (string) ($request['path'] ?? 'php://temp');
        $contents = file_get_contents($path) ?: $payload;
        $value = unserialize($contents, ['allowed_classes' => true]);
        return is_scalar($value) ? (string) $value : get_debug_type($value);
    }

    private function handleFailure(Throwable $e, string $payload): string
    {
        return 'file-fallback:' . strlen($payload) . ':' . $e->getMessage();
    }

    public function middleware(array $request): string
    {
        return $this->handle($request);
    }
}

final class PhpLarge030Tail
{
    public function transform(string $value): string
    {
        return trim($value);
    }
}
