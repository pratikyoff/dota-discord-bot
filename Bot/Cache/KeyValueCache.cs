using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot
{
    public static class KeyValueCache
    {
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();
        private static List<string> _keys = new List<string>();
        private static int _limit = 100;

        public static string Get(string key)
        {
            if (_cache.ContainsKey(key))
                return _cache[key];
            else return null;
        }

        public static bool Put(string key, string value)
        {
            if (Get(key) == null)
            {
                if (_keys.Count >= _limit)
                {
                    _cache.Remove(_keys.Last());
                    _keys.RemoveAt(_keys.Count);
                }
                _cache[key] = value;
                _keys.Add(key);
                return true;
            }
            else
                return false;
        }
    }
}
