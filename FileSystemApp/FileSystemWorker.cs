using FileSystemFrame.Objects;
using FileSystemFrame.Open;
using FileSystemFrame.Open.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileSystemApp 

 {
   public class FileSystemWorker
    {
        private readonly IFileSystem _fileSystem;
        private HashSet<string> _validCommands;
        public FileSystemWorker(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public void ReadWholeFileSystem()
        {
            Console.WriteLine("Enter the file path:");
            var path = Console.ReadLine()?.Trim();
            try
            {
                var data = _fileSystem.ReadFile(path);
                _fileSystem.LoadFileSystem(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+"\n");
                ReadWholeFileSystem();
            }
        }

        public string WriteCommand()
        {
            while (true)
            {                
                Console.WriteLine("\nWrite the Action you want to do:");
                _validCommands = new HashSet<string> { "help", "superblock", "fat", "root", "exit", "data logic", "read file"};
                var action = Console.ReadLine()?.ToLower().Trim();
                if (!_validCommands.Contains(action))
                {
                    Console.WriteLine("Invalid command. Type 'help' for a list of commands. \n");
                }
                else return action;
            }
        }

        public void SelectTheAction() 
        {   
            while(true)
            { 
            var action = WriteCommand();
            Console.WriteLine($"\nExecuting command: {action}\n");
                switch (action)
                {
                    case "help":
                        Console.WriteLine("List of commands: ");
                        //\n - help; \n - Superblock; \n - Fat; \n - Root; \n - Exit; \n - Data logic \n
                        foreach(var s in _validCommands)
                        {
                            Console.WriteLine(s);
                        }
                        break;
                    case "superblock":
                        Console.WriteLine($"Block size = {_fileSystem.GetSuperblock().BlockSize}");
                        Console.WriteLine($"Fat records size = {_fileSystem.GetSuperblock().FatSize}");
                        Console.WriteLine($"Root directory entries amount = {_fileSystem.GetSuperblock().RootRecordsAmount} \n");
                        break;
                    case "fat":
                        var fatTable = _fileSystem.GetFatTable();
                        fatTable.ReadFatTable.ToList().ForEach(e => Console.WriteLine($"Key: {e.Key}, Value: {e.Value} \n"));
                        break;
                    case "root":
                        var rootDir = _fileSystem.GetRootDirObject();
                        rootDir.ReadFileDirectory.ForEach(e => Console.WriteLine($"File name: {e.FileName}, First block: {e.FirstBlock}, Attribute: {e.Attribute} \n"));
                        break;
                    case "read file":
                        Console.WriteLine("Enter the file name:");
                        var fileName = Console.ReadLine();
                        Console.WriteLine(_fileSystem.ReadFileContent(fileName));
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Command doesn't exist, in order to clarify, type 'help' \n");
                        SelectTheAction();
                        break;
                }
            }
        }
    }
}
