using FileSystemFrame.Exceptions;
using System.Reflection.Metadata;
using System.Text;

namespace FileSystemFrame.Objects
{
    public class DirObject
    {
        private readonly byte[] _data;
        private const int ENTRY_SIZE = 20;
        internal DirObject(byte[] data)
        {
            if (data == null)
                throw new RootDirInvalidException("Root directory data is null");

            _data = data;
        }

        public FileEntry GetFileEntry(List<FileEntry> fileEntries, string fileName)
        {
            return fileEntries.FirstOrDefault(f => f.FileName.Contains(fileName))
                        ?? throw new DirectoryNotFoundException($"{fileName} not found");
        }

       

        public List<FileEntry> ReadDirectory()
        {
            var records = new List<FileEntry>();
            for (int i = 0; i < _data.Length / ENTRY_SIZE; i++)
            {
                var fileRowBytes = _data.Skip(i * ENTRY_SIZE).Take(ENTRY_SIZE).ToArray();

                var fileNameBytes = fileRowBytes.Take(12).ToArray();
                var blockNumberBytes = fileRowBytes.Skip(12).Take(4).ToArray();
                var attributeBytes = fileRowBytes.Skip(16).Take(4).ToArray();

                var fileName = Encoding.UTF8.GetString(fileNameBytes);
                var firstBlock = BitConverter.ToUInt32(blockNumberBytes);
                var attribute = BitConverter.ToInt32(attributeBytes);
                if (fileName != null && firstBlock != 0)
                {
                    var record = new FileEntry()
                    {
                        FileName = fileName,
                        FirstBlock = firstBlock,
                        Attribute = attribute
                    };
                    records.Add(record);
                }
            }
            return records;
        }

        public void ReadFile()
        {
            var fileList = ReadDirectory();
            //fileList()
        }

    }

    public class FileEntry
    {
        public string FileName { get; set; }
        public uint FirstBlock { get; set; }
        public int Attribute { get; set; }
    }
}
