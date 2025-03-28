
using FileSystemFrame.Objects;
using FileSystemFrame.Open.DTO;

namespace FileSystemFrame.Open
{
    public interface IFileSystem
    {
        /// <summary>
        /// Read file located on the path and reading bytes from it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        byte[] ReadFile(string path);

        /// <summary>
        /// Load file system from byte array
        /// </summary>
        /// <param name="data"></param>
        void LoadFileSystem(byte[] data);

        /// <summary>
        /// Get data from superblock area of file system
        /// </summary>
        /// <returns></returns>
        SuperblockDTO GetSuperblock();

        /// <summary>
        /// Get data about bytes from FAT table
        /// </summary>
        /// <returns></returns>
        FATTable GetFatTable();

        /// <summary>
        /// Get data about directories inside file system
        /// </summary>
        /// <returns></returns>
        DirObject GetDirObject();

        /// <summary>
        /// Read file content from files inside file system
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ReadFileContent(string path);
    }
}
