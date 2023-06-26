using System.Collections.Generic;
using System.IO;
using System.Linq;

public static partial class Main
{
    public static class Strings
    {
        public static string GetCompactedNumber(ulong number)
        {
            if (number >= 1000)
            {
                return string.Format("{0:N0}k", number / 1000);
            }
            else if (number >= 1000000)
            {
                return string.Format("{0:N0}m", number / 1000000);
            }
            else
            {
                return number.ToString();
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static Stream Stream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}