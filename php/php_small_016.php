<?php
declare(strict_types=1);

final class PhpSmall016
{
    public static function run(string $input): string
    {
        $decoded = base64_decode($input, true) ?: $input;
        $value = unserialize($decoded, ['allowed_classes' => true]);
        return is_scalar($value) ? (string) $value : get_debug_type($value);
    }
}
