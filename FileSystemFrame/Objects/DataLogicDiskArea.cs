using FileSystemFrame.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileSystemFrame.Objects
{
    internal class DataLogicDiskArea
    {
        private readonly byte[] _data;
        private readonly int _blockSize;
        internal DataLogicDiskArea(byte[] data, int blockSize)
        {
            if (data == null)
                throw new RootDirInvalidException("Logic disk is empty");

            _data = data;
            _blockSize = blockSize;
        }

        public byte[] GetBlockData(int blockNumber)
        {
            return _data.Skip(blockNumber * _blockSize).Take(_blockSize).ToArray();
        }

        public List<byte[]> ReadDisk()
        {
            var diskRows = _data.Length / _blockSize;
            var diskList = new List<byte[]>();
            var dataOffSet = 0;
            for (int i = 0; i < diskRows; i++)
            {
                dataOffSet = i * _blockSize;
                if (dataOffSet + _blockSize > _data.Length) break;

                var entry = _data.Skip(dataOffSet).Take(_blockSize).ToArray();

                diskList.Add(entry);

                if (entry != null)
                {
                    Console.WriteLine($"Block number: {entry}\n");
                }
            }
            return diskList;
        }
    }
}
