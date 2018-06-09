using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Utils  {

    private static string[] prop = {"Red", "Green", "Blue", "Circle", "Square", "Triangle"};
    private static Random rand = new Random();

    public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            yield return values[rand.Next(size)];
        }
    }
    
    public static TValue RandomValue<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        foreach (TValue a in RandomValues(dict).Take(1))
        {
            return a;
        }
        return default(TValue);
    }

    public static string RandomProperty()
    {
        return prop[rand.Next(prop.Length)];
    }
}
