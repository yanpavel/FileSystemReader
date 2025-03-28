using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFrame.Open.DTO
{
    public class SuperblockDTO
    {
        public uint BlockSize { get; set; }

        public uint FatSize { get; set; }

        public uint RootRecordsAmount { get; set; }
    }
}
