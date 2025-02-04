using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemApp
{
    class FATTable : Superblock
    {
        public void ReadFatTable()
        {
            var fatRows = FatRecordsAmount / 8;
            var fatTable = new int[fatRows, 2];
            var dataOffSet = 0;
            for (int i = 0; i < fatRows; i++)
            {
                dataOffSet = SuperblockSize + i * 8;
                fatTable[i, 0] = BitConverter.ToInt32(_data, dataOffSet);
                fatTable[i, 1] = BitConverter.ToInt32(_data, dataOffSet+4);
                Console.WriteLine($"Block number: {fatTable[i, 0]}");
                Console.WriteLine($"Table content: {fatTable[i, 1]}");
            }
        }
    }
}
