using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Bot.Universal
{
    public static class ReflectiveEnumerator
    {
        public static List<T> GetInheritedFromAbstractClass<T>(params object[] constructorArgs) where T : class
        {
            var subclassTypes = Assembly.GetAssembly(typeof(T)).GetTypes().Where(x => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(T)));
            List<T> subclasses = new List<T>();
            foreach (var subclassType in subclassTypes)
            {
                subclasses.Add((T)Activator.CreateInstance(subclassType, constructorArgs));
            }
            return subclasses;
        }

        public static List<T> GetInheritedFromInterface<T>(params object[] constructorArgs)
        {
            if (!typeof(T).IsInterface) return null;
            var assembly = Assembly.GetAssembly(typeof(T));
            List<T> implementedClasses = new List<T>();
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.ImplementedInterfaces.Contains(typeof(T)))
                {
                    implementedClasses.Add((T)assembly.CreateInstance(typeInfo.FullName));
                }
            }
            return implementedClasses;
        }
    }
}
