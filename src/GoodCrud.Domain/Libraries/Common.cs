using System.Collections.Generic;

namespace GoodCrud.Domain.Libraries
{
    public class Common
    {
        public static List<string> GetStringList(string raw, bool sort = true)
        {
            var result = new List<string>();
            if (raw == null) { return result; }

            foreach (var s in raw.Split(","))
            {
                if (string.IsNullOrWhiteSpace(s)) { continue; }
                result.Add(s.Trim());
            }
            if (sort) { result.Sort(); }

            return result;
        }

    }
}