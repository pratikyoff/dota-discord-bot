using System;

namespace Bot
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Command : Attribute
    {
        public string CommandText { get; private set; }

        public Command(string commandText)
        {
            CommandText = commandText;
        }
    }
}
