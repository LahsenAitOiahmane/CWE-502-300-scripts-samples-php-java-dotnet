# CWE-502 Insecure Deserialization Benchmark Samples

This folder contains a benchmark dataset of source files in PHP, Java, and C# designed for testing SAST (Static Application Security Testing) tools against CWE-502 (Insecure Deserialization).

## Folder Structure

The code samples are categorized by programming language and complexity (large, medium, small):

- **`php/`**: PHP classes containing `unserialize` usages and related magic methods (`__wakeup`, `__destruct`, `__toString`).
- **`java/`**: Java classes demonstrating insecure deserialization via `ObjectInputStream`, `XMLDecoder`, Jackson, Fastjson, and XStream.
- **`cs/`**: C# classes showing insecure deserialization using `BinaryFormatter`, `JsonSerializerSettings` (TypeNameHandling), `LosFormatter`, `NetDataContractSerializer`, and `JavaScriptSerializer`.
