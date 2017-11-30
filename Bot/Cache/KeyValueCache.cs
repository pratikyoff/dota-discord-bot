using System;
using System.Collections.Generic;
using System.Text;

namespace Bot
{
    public static class KeyValueCache
    {
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        public static string Get(string key)
        {
            if (_cache.ContainsKey(key))
                return _cache[key];
            else return null;
        }

        public static bool Put(string key, string value)
        {
            if (Get(key) != null)
            {
                _cache[key] = value;
                return true;
            }
            else
                return false;
        }
    }
}
