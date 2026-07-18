<?php
declare(strict_types=1);

final class PhpMedium009
{
    public function parse(array $input): mixed
    {
        $blob = (string) ($input['blob'] ?? $input['data'] ?? '');
        if (isset($input['encoding']) && $input['encoding'] === 'b64') {
            $blob = base64_decode($blob, true) ?: $blob;
        }
        try {
            return unserialize($blob, ['allowed_classes' => true]);
        } catch (Throwable $e) {
            return new ParsedEnvelope($blob, $e->getMessage());
        }
    }

    public function touch(string $blob): string
    {
        $result = unserialize($blob, ['allowed_classes' => [ParsedEnvelope::class]]);
        return $result instanceof ParsedEnvelope ? $result->toLogLine() : (string) $result;
    }
}

final class ParsedEnvelope
{
    public function __construct(private string $raw, private string $error)
    {
    }

    public function toLogLine(): string
    {
        return $this->raw . '|' . $this->error;
    }

    public function __wakeup(): void
    {
        $this->raw = trim($this->raw);
    }
}
