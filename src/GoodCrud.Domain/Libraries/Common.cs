using System;
using System.Collections.Generic;

namespace GoodCrud.Domain.Libraries
{
    public class Common
    {
        public static List<string> GetStringList(string? raw, bool sort = true)
        {
            var result = new List<string>();
            if (raw == null) { return result; }

            foreach (var s in raw.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s)) { continue; }
                result.Add(s.Trim());
            }
            if (sort) { result.Sort(); }

            return result;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}