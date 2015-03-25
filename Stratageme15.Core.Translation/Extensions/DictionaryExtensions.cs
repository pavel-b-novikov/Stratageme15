using System.Collections.Generic;

namespace Stratageme15.Core.Translation.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (!dictionary.ContainsKey(key))
            {
                var val = new TValue();
                dictionary[key] = val;
                return val;
            }
            return dictionary[key];
        }
    }
}