using Bot.Contracts;
using Bot.Implementations;
using System;
using System.Collections.Generic;
using Bot.Universal;
using System.Linq;

namespace Bot.Configuration
{
    public static class CommandConfiguration
    {
        public static Dictionary<string, ICommand> Get { get; } = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);

        public static string GetCommandText<T>()
        {
            string commandText = string.Empty;
            var attribute = typeof(T).GetCustomAttributes(typeof(Command), false).FirstOrDefault() as Command;
            if (attribute == null) return commandText;
            commandText = attribute.CommandText;
            return commandText;
        }

        public static void Initialize()
        {
            var commandClasses = ReflectiveEnumerator.GetInheritedFromInterface<ICommand>();
            foreach (var commandClass in commandClasses)
            {
                var commandAttribute = commandClass.GetType().GetCustomAttributes(typeof(Command), false).FirstOrDefault() as Command;
                if (commandAttribute == null) continue;
                Get.Add(commandAttribute.CommandText, commandClass);
            }
        }
    }
}
