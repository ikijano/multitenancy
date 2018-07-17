using System.Collections.Generic;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> map, TKey key, TValue value)
        {
            if (map.ContainsKey(key))
                map[key] = value;
            else
                map.Add(key, value);
        }
    }
}