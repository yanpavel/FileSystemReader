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
            var currentDir = GetDirObject();
            var pathWithoutFirstSlash = path.Trim('/');
            var listOfFolders = pathWithoutFirstSlash.Split('/');

            if (listOfFolders.Length > 1)
            {
                for (var i = 0; i < listOfFolders.Length - 1; i++)
                {
                    var folderName = listOfFolders[i];
                    var dirList = currentDir.ReadDirectory();
                    if (!dirList.Any(f=>f.FileName.Contains(folderName))) 
                        throw new DirectoryNotFoundException($"Directory not found, current directory is {folderName}");
                    var folder = dirList.First(f => f.FileName.Contains(folderName));
                    var dirChains = ReadChains(folder.FirstBlock);
                    var dirBytes = ReadByteByChains(dirChains);
                    currentDir = new DirObject(dirBytes);
                }
            }
            
            var fileEntry = currentDir.ReadDirectory().FirstOrDefault(f => f.FileName.Contains(listOfFolders.Last()));
            if(fileEntry == null) 
                throw new NullReferenceException("First block is null");

            var chains = ReadChains(fileEntry.FirstBlock);
            var byteData = ReadByteByChains(chains);

            if (fileEntry.FileName.Contains(".txt"))
            {
                string textContent = Encoding.UTF8.GetString(byteData);
                return textContent;
            }
            else if (fileEntry.FileName.Contains(".img") || fileEntry.FileName.Contains(".jpg") || fileEntry.FileName.Contains(".c"))
            {
                var filePath = Path.GetTempFileName();
                File.WriteAllBytes(filePath, byteData);
                OpenFile(filePath);
                return "File opened";
            }
            else
            {
                var dirChains = ReadChains(fileEntry.FirstBlock);
                var dirBytes = ReadByteByChains(dirChains);
                currentDir = new DirObject(dirBytes);
                var dirContent = currentDir.ReadDirectory();
                dirContent.ForEach(f => Console.WriteLine($"File name: {f.FileName}, First block: {f.FirstBlock}, Attribute: {f.Attribute}"));
                return "Directory content";
            }

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
