using System.IO;
using System.Linq;

namespace Bot.Universal
{
    public static class FileOperations
    {
        public static int GetTotalNoOfLinesInFile(string fileName)
        {
            int lineCount;
            try
            {
                lineCount = File.ReadLines(fileName).Count();
            }
            catch
            {
                return -1;
            }
            return lineCount;
        }

        public static string ReadLineNo(string fileName, int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= GetTotalNoOfLinesInFile(fileName))
                return null;
            return File.ReadLines(fileName).ElementAt(lineIndex);
        }

        public static bool AppendLine(string fileName, string line)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(line);
            }
            return true;
        }

        public static bool DeleteLine(string fileName, int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= GetTotalNoOfLinesInFile(fileName))
                return false;
            string line = null;
            using (StreamReader sr = new StreamReader(fileName))
            {
                using (StreamWriter sw = new StreamWriter("temp.log"))
                {
                    for (int i = 0; (line = sr.ReadLine()) != null; i++)
                    {
                        if (i == lineIndex) continue;
                        sw.WriteLine(line);
                    }
                }
            }
            File.Delete(fileName);
            File.Move("temp.log", fileName);
            return true;
        }
    }
}
