using FileSystemFrame.Exceptions;
using FileSystemFrame.Open.DTO;
using System.Reflection.Metadata;
using System.Text;

namespace FileSystemFrame.Objects
{
    internal class DirObject
    {
        private readonly byte[] _data;
        private const int ENTRY_SIZE = 20;
        internal DirObject(byte[] data)
        {
            if (data == null)
                throw new RootDirInvalidException("Root directory data is null");

            _data = data;
        }

        public FileEntryDTO GetFileEntry(List<FileEntryDTO> fileEntries, string fileName)
        {
            return fileEntries.FirstOrDefault(f => f.FileName.Contains(fileName))
                        ?? throw new DirectoryNotFoundException($"{fileName} not found");
        }

       

        public List<FileEntryDTO> ReadDirectory()
        {
            var records = new List<FileEntryDTO>();
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
                    var record = new FileEntryDTO()
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
}
