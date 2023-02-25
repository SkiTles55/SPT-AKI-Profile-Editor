using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class ExtMethods
    {
        public static string WindowsCulture => CultureInfo.CurrentCulture.Parent.ToString();

        public static double GetTimestamp => Math.Floor(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

        public static string GenerateNewId(IEnumerable<string> ids)
        {
            string id;
            do
            {
                var getTime = DateTime.Now;
                Random rnd = new();
                var random = rnd.Next(100000000, 999999999).ToString();
                var retVal = $"{getTime:MM}{getTime:dd}{getTime:HH}{getTime:mm}{getTime:ss}{random}";
                var sign = MakeSign(24 - retVal.Length, rnd);
                id = retVal + sign;
            } while (ids.Contains(id));
            return id;
        }

        private static string MakeSign(int length, Random random)
        {
            var result = "";
            var characters = "0123456789abcdef";
            for (int i = 0; i < length; i++)
                result += characters.ElementAt((int)Math.Floor(random.NextDouble() * characters.Length));
            return result;
        }
    }
}