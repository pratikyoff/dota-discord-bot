using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Bot.Universal
{
    public static class ReflectiveEnumerator
    {
        public static List<T> GetInheritedClasses<T>(params object[] constructorArgs) where T : class
        {
            var subclassTypes = Assembly.GetAssembly(typeof(T)).GetTypes().Where(x => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(T)));
            List<T> subclasses = new List<T>();
            foreach (var subclassType in subclassTypes)
            {
                subclasses.Add((T)Activator.CreateInstance(subclassType, constructorArgs));
            }
            return subclasses;
        }
    }
}
