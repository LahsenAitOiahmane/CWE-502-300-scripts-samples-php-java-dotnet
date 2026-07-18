# CWE-502 Insecure Deserialization Benchmark Samples

This folder contains a benchmark dataset of source files in PHP, Java, and C# designed for testing SAST (Static Application Security Testing) tools against CWE-502 (Insecure Deserialization).

## Folder Structure

The code samples are categorized by programming language and complexity (large, medium, small):

- **`php/`**: PHP classes containing `unserialize` usages and related magic methods (`__wakeup`, `__destruct`, `__toString`).
- **`java/`**: Java classes demonstrating insecure deserialization via `ObjectInputStream`, `XMLDecoder`, Jackson, Fastjson, and XStream.
- **`cs/`**: C# classes showing insecure deserialization using `BinaryFormatter`, `JsonSerializerSettings` (TypeNameHandling), `LosFormatter`, `NetDataContractSerializer`, and `JavaScriptSerializer`.

## Dataset Generation

The dataset files (and their compressed archive `cwe502_insecure_deserialization_benchmark.zip`) can be generated programmatically.

### Python (Recommended for WSL/Linux)
Run the generator script with Python 3:
```bash
python3 generate_cwe502_dataset.py
```

### PowerShell (Windows)
Run the script in a PowerShell session:
```powershell
.\generate_cwe502_dataset.ps1
```
*(Note: If running on Windows, you may need to bypass execution policies or ensure Controlled Folder Access does not block the script from writing to protected directories).*
