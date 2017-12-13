using Bot.Universal;
using System.IO;
using Xunit;

namespace BotFixture
{
    public class FileOperationsFixture
    {
        [Fact]
        public void TotalLineFixture()
        {
            string filename = "newfile.txt";
            File.Delete(filename);
            FileOperations.AppendLine(filename, filename);
            FileOperations.AppendLine(filename, filename);
            FileOperations.AppendLine(filename, filename);
            Assert.Equal(3, FileOperations.GetTotalNoOfLinesInFile(filename));
            File.Delete(filename);
        }

        [Fact]
        public void DeleteLineFixture()
        {
            string filename = "newfile.txt";
            File.Delete(filename);
            FileOperations.AppendLine(filename, "1");
            FileOperations.AppendLine(filename, "2");
            FileOperations.AppendLine(filename, "3");
            FileOperations.DeleteLine(filename, 1);
            Assert.Equal("3", FileOperations.ReadLineNo(filename, 1));
            File.Delete(filename);
        }

        [Fact]
        public void ReadLineFixture()
        {
            string filename = "newfile.txt";
            File.Delete(filename);
            FileOperations.AppendLine(filename, "1");
            FileOperations.AppendLine(filename, "2");
            FileOperations.AppendLine(filename, "3");
            FileOperations.AppendLine(filename, "4");
            Assert.Equal("4", FileOperations.ReadLineNo(filename, 3));
            File.Delete(filename);
        }
    }
}
