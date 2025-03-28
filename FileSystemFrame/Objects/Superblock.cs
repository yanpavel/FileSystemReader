

using FileSystemFrame.Exceptions;

namespace FileSystemFrame.Objects
{
    public class Superblock
    {
        private readonly byte[] _data;
        internal const int SuperblockSize = 12;
        public Superblock(byte[] data)
        {
            if (data == null)
                throw new SuperblockInvalidException("Superblock data is null");
            if (data.Length != SuperblockSize)
                throw new SuperblockInvalidException($"Superblock data is not equal to {SuperblockSize}");

            _data = data;
        }

        internal uint BlockSize => BitConverter.ToUInt32(_data, 0);

        internal uint FatTableSize => BitConverter.ToUInt32(_data, 4);

        internal uint RootDirRecords => BitConverter.ToUInt32(_data, 8);
    }
}
