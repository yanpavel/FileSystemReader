using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFrame.Open.DTO
{
    public class FileEntryDTO
    {
        public string FileName {  get; set; }
        public uint FirstBlock { get; set; }
        public int Attribute {  get; set; }
    }
}
