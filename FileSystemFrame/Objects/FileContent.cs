
using System.Runtime.CompilerServices;

namespace FileSystemFrame.Objects
{
    internal class FileContent
    {
        protected static byte[] _data = null!;

        internal FileContent(byte[] data) 
        {
            _data = data;
        }
        
        internal uint GetBlockSize() => new Superblock(GetSuperBlockBytes()).BlockSize;
        internal byte[] GetSuperBlockBytes() => _data.Take(Superblock.SuperblockSize).ToArray();
        internal byte[] GetFatTableBytes()
        {
            var superblock = new Superblock(GetSuperBlockBytes());
            return _data.Skip(Superblock.SuperblockSize).Take((int)superblock.FatTableSize).ToArray();

        }
        internal byte[] GetRootDirBytes()
        {
            var superblock = new Superblock(GetSuperBlockBytes());
            return _data.Skip(Superblock.SuperblockSize).Skip((int)superblock.FatTableSize).Take((int)superblock.RootDirRecords * 20).ToArray();
        }
        internal byte[] GetDataLogicBytes()
        {
            var superblock = new Superblock(GetSuperBlockBytes());
            return _data.Skip(Superblock.SuperblockSize).Skip((int)superblock.FatTableSize)
            .Skip((int)superblock.RootDirRecords * 20).ToArray();
        }

    }
}
