using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Implementations
{
    class NoLogger : ILogger
    {
        public void Log(string log)
        {
        }
    }
}
