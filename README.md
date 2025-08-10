# ​ FileSystemReader

**Console application for reading `.dat` files**, written in C#. Parses and displays content from binary `.dat` formats for analysis and troubleshooting.

---

##  Project Structure

├── FileSystemApp/ — Main application source code
├── FileSystemFrame/ — Shared library or core components
├── FileSystemTests/ — Unit tests for application logic
├── FileSystemApp.sln — Visual Studio solution file
└── output.txt — Sample output or test data

---

##  Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (e.g., .NET 6+)
- A `.dat` file to read (binary data format relevant to the application)

### Build and Run

```bash
# Clone the repository
git clone https://github.com/yanpavel/FileSystemReader.git
cd FileSystemReader/FileSystemApp

# Build and run the application
dotnet run --project FileSystemApp.sln path/to/yourfile.dat
```
You should see parsed data (or structured representation) printed to the console or logged in output.txt.

---

## Testing

Unit tests are available in the FileSystemTests project:
```bash
dotnet test FileSystemTests/FileSystemTests.csproj
```

Run these to verify parsing logic and ensure correctness for various .dat file scenarios.

---

## Future Improvements
- Add support for multiple .dat structures or file formats
- Implement error handling for corrupted or missing files
- Enhance output with options to export data to JSON or CSV
- Add CLI arguments parsing (e.g., output paths, verbosity levels)
