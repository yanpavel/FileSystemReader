using FileSystemApp;
using System.Reflection.PortableExecutable;

namespace FileSystemApp
{ 
    class Program
    {

    static void Main()
        {
            Console.WriteLine("Enter the file path:");
            string filePath = Console.ReadLine();
            var s = new RootDir();
            s.ReadWholeFileSystem(filePath);
            s.ReadSuperBlockBytes();
            s.SizeAndOffset();
            s.OutputRootDirData();
            //f.ReadSuperBlockBytes();
            // f.ReadFatTable();
            /*var f = new RootDir();
            f.ReadWholeFileSystem(filePath);
            f.ReadSuperBlockBytes();
            f.OutputRootDirData();
            */
        }
    }
}

    