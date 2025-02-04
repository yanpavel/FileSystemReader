using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemApp
{
    class RootDir : Superblock
    {

        int dataOffSet;
        public void OutputRootDirData()
        {
            for (int i = 0; i < RootDirRecordsAmount; i++)
            {
                if(RootDirOffset+RootDirRecordsAmount*20>_data.Length) Console.WriteLine("Size of Root directory exceeds size of the file");
                dataOffSet = (int)RootDirOffset + i * 20;
                ReadRecordDir(dataOffSet);
            }
        }

        private void ReadRecordDir(int dataOffset)
        {
            var fileName = Encoding.UTF8.GetString(_data.Skip(dataOffset).Take(12).ToArray()).Replace("\0","");
            var blockNumber = BitConverter.ToUInt32(_data, dataOffset+12);
            var attributes = BitConverter.ToInt32(_data, dataOffset + 12 +4);
            if (attributes == 0)
            {
                Console.WriteLine($"File name: {fileName}");
            }
            else if (attributes == 1) { Console.WriteLine($"Catalog name: {fileName}"); }
            Console.WriteLine($"Block's first number: {blockNumber}");
        }
    }
}
