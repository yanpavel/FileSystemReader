using FileSystemFrame.Open;
using FileSystemFrame.Open.DTO;
using FluentAssertions;

namespace FileSystemTests
{
    [TestFixture]
    public class SuperblockTests
    {
        IFileSystem fileSystem;
        byte[] data;

        [SetUp]
        public void Setup()
        {
            fileSystem = new FileSystem();
            data = fileSystem.ReadFile("./TestData/v13.dat");
            fileSystem.LoadFileSystem(data);
        }

        [Test, Description("Проверка данных в суперблоке")]
        public void ValidSuperBlockTest()
        {
            var superblock = fileSystem.GetSuperblock();
            superblock.Should().NotBeNull();
            superblock.BlockSize.Should().Be(2048);
            superblock.FatSize.Should().Be(4096);
            superblock.RootRecordsAmount.Should().Be(16);
        }

        [Test, Description("Проверка данных в Fat")]
        public void ValidFatTableTest()
        {
            var fatTable = fileSystem.GetFatTable();
            fatTable.Should().NotBeNull();

            foreach (var f in fatTable.ReadFatTable())
            {
                Console.Write(f.Key + " ");
                f.Key.Should().BeInRange(-1024, 1024);
                Console.WriteLine(f.Value);
                f.Value.Should().BeInRange(-1024, 1024);
            }
        }

        [Test, Description("Проверка данных в Root dir")]
        public void ValidRootDir()
        {
            var rootDir = fileSystem.GetDirObject();
            foreach (var r in rootDir.ReadDirectory())
            {
                switch (r.Attribute)
                {
                    case 0:
                        Console.WriteLine($"File name:{r.FileName}");
                        Console.WriteLine($"First block:{r.FirstBlock}");
                        break;
                    case 1:
                        Console.WriteLine($"Directory name:{r.FileName}");
                        Console.WriteLine($"First block:{r.FirstBlock}");
                        break;
                }
                r.FileName.Should().NotBeNullOrEmpty();
                r.FirstBlock.Should().BeInRange(0, 1024);
                r.Attribute.Should().BeInRange(0, 1);
            }
            rootDir.Should().NotBeNull();
        }


        [Test, Description("Проверка данных внутри Root directory")]
        public void ValidRootFileInfo()
        {
            var content = fileSystem.ReadFileContent("/w13.jpg");
            content.Should().NotBeNullOrEmpty();
        }
    }
}