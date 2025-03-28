

using FileSystemFrame.Exceptions;

namespace FileSystemFrame.Objects
{
    public class FATTable
    {
        private readonly byte[] _data;
        public FATTable(byte[] data) 
        {
            if (data == null)
                throw new FatTableInvalidException("Fat table data is null");

            _data = data;
        }
        public Dictionary<int, int> ReadFatTable()
        {
            var fatRows = _data.Length / 8;
            var fatTable = new Dictionary<int, int>();
            var dataOffSet = 0;
            for (int i = 0; i < fatRows; i++)
            {
                dataOffSet = i * 8;
                if (dataOffSet + 8 > _data.Length) break;

                int key = BitConverter.ToInt32(_data, dataOffSet);
                int value = BitConverter.ToInt32(_data, dataOffSet + 4);

                fatTable.Add(key, value);

                if (value!= 0)
                {
                    Console.WriteLine($"Block number: {key}");
                    Console.WriteLine($"Table content: {value}\n");
                }
            }
            return fatTable;
        }
    }
}
