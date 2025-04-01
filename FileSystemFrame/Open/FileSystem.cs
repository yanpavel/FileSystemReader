using FileSystemFrame.Objects;
using FileSystemFrame.Open.DTO;
using System;
using System.Linq;
using System.Text;

namespace FileSystemFrame.Open
{
    public class FileSystem : IFileSystem
    {
        private FileContent fileContent = null!;
        public SuperblockDTO GetSuperblock()
        {
            if (fileContent == null) 
                throw new ArgumentNullException("File content is empty");
            
            var superblock = new Superblock(fileContent.GetSuperBlockBytes());

            return new SuperblockDTO
            {
                BlockSize = superblock.BlockSize,
                FatSize = superblock.FatTableSize,
                RootRecordsAmount = superblock.RootDirRecords
            };
        }

        public FATTable GetFatTable()
        {
            if (fileContent == null) throw new Exception("file content is empty");
            var fatTable = new FATTable(fileContent.GetFatTableBytes());
            return fatTable;
        }

        public DirObject GetDirObject()
        {
            if (fileContent == null) throw new Exception("file content is empty");
            var rootDir = new DirObject(fileContent.GetRootDirBytes());
            return rootDir;
        }
        public void LoadFileSystem(byte[] data)
        {
            fileContent = new FileContent(data);
        }

        public byte[] ReadFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"File was not found at this path: {path}");
            else if (Path.GetExtension(path) != ".dat")
            {
                var mes = "Invalid file extension";
                throw new FormatException(mes);
            }
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Cannot read file system" + ex.Message);
            }
        }

        public string ReadFileContent(string path)
        {
            string[] listOfFolders = FoldersNameArray(path);

            var currentDir = GetDirObject();

            if (listOfFolders.Length > 1)
            {
                for (var i = 0; i < listOfFolders.Length - 1; i++)
                {
                    var folderName = listOfFolders[i];

                    var entry = GetEntry(currentDir, folderName);
                    var dirBytes = GetByteArrayFromEntry(entry);

                    currentDir = new DirObject(dirBytes);
                }
            }

            var entryLast = GetEntry(currentDir, listOfFolders.Last());
            var entryData = GetByteArrayFromEntry(entryLast);
            return ProsessFileEntry(ref currentDir, entryLast, entryData);

        }

        private string ProsessFileEntry(ref DirObject currentDir, FileEntry entryLast, byte[] entryData)
        {
            if (entryLast.FileName.Contains(".txt"))
            {
                string textContent = Encoding.UTF8.GetString(entryData);
                return textContent;
            }
            else if (entryLast.FileName.Contains('.'))
            {
                var filePath = Path.GetTempFileName();
                File.WriteAllBytes(filePath, entryData);
                OpenFile(filePath);
                return "File opened";
            }
            else
            {
                currentDir = new DirObject(entryData);
                var dirContent = currentDir.ReadDirectory();
                dirContent.ForEach(f => Console.WriteLine($"File name: {f.FileName}, First block: {f.FirstBlock}, Attribute: {f.Attribute}"));
                return "Directory content";
            }
        }

        private byte[] GetByteArrayFromEntry(FileEntry entry)
        {
            var dirChains = ReadChains(entry.FirstBlock);
            var dirBytes = ReadByteByChains(dirChains);
            return dirBytes;
        }

        private static FileEntry GetEntry(DirObject currentDir, string folderName)
        {
            var dirList = currentDir.ReadDirectory();
            var entry = currentDir.GetFileEntry(dirList, folderName);
            return entry;
        }

        private static string[] FoldersNameArray(string path)
        {
            var pathWithoutFirstSlash = path.Trim('/') 
                ?? throw new DirectoryNotFoundException("Path is empty");
            var listOfFolders = pathWithoutFirstSlash.Split('/');
            return listOfFolders;
        }

        private List<int> ReadChains(uint firstBlock)
        {
            var fatTable = GetFatTable();
            List<int> chains = [(int)firstBlock];
            int currentBlock = (int)firstBlock;
            while (true)
            {
                var newEntry = fatTable.ReadFatTable().FirstOrDefault(k=>k.Key==currentBlock);
                var nextBlock = newEntry.Value;
                if (nextBlock < 0)
                    break;

                chains.Add(nextBlock);
                currentBlock = nextBlock;
            }
            return chains;
        }

        private byte[] ReadByteByChains(List<int> chains)
        {
            var data = new List<byte>();
            var logicArea = new DataLogicDiskArea(fileContent.GetDataLogicBytes(), (int)GetSuperblock().BlockSize);
            foreach (var block in chains)
            {
                var byteData = logicArea.GetBlockData(block);
                data.AddRange(byteData);
            }
            return data.ToArray();
        }

        private void OpenFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open file: {ex.Message}");
            }
        }
    }
}
