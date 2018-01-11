using System;

namespace Bot
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Command : Attribute
    {
        public string CommandText { get; private set; }

        public Command(string commandText)
        {
            CommandText = commandText;
        }
    }
}
