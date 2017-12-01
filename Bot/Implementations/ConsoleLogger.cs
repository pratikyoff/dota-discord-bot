using Bot.Contracts;
using System;

namespace Bot.Implementations
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}
