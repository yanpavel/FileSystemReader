
namespace FileSystemApp
{
    class Superblock
    {
        protected const int SuperblockSize = 12;
        int _blockByte = 4;
        int _fatByte = 4;
        int _rootDirByte = 4;
        protected byte[] _data = Array.Empty<byte>();

        protected static uint RootDirOffset { get { return SuperblockSize + FatRecordsAmount; } }
        protected static uint BlockRecordsAmount { get; set; } //Размер блока
        protected static uint FatRecordsAmount { get; set; } //Размер в байтах
        protected static uint RootDirRecordsAmount { get; set; }//Количество записей
        static uint RootSize { get { return RootDirRecordsAmount * 20; } }


        public void ReadWholeFileSystem(string path)
        {
            _data = File.ReadAllBytes(path);
        }

        public void ReadSuperBlockBytes()
        {
            BlockRecordsAmount = BitConverter.ToUInt32(_data, 0);
            FatRecordsAmount = BitConverter.ToUInt32(_data, _blockByte);
            RootDirRecordsAmount = BitConverter.ToUInt32(_data, _blockByte+ _fatByte);
            Console.WriteLine($"Block size: {BlockRecordsAmount}");
            Console.WriteLine($"FAT size: {FatRecordsAmount}");
            Console.WriteLine($"Root directory size: {RootDirRecordsAmount}");
        }

        public void SizeAndOffset()
        {
            Console.WriteLine($"RootDirOffset: {RootDirOffset}");
        }
    }
}
