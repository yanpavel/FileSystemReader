using FileSystemFrame.Open;

namespace FileSystemApp
{ 
    class Program
    {
        static void Main()
        {
            var fileSystem = new FileSystem();
            var worker = new FileSystemWorker(fileSystem);
            worker.ReadWholeFileSystem();
            worker.SelectTheAction();

        }
    }
}

    