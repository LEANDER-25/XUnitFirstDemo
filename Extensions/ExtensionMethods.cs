using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XUnitDemo.Extensions
{
    public static class ExtensionMethods
    {
        public static void LogOutList(this IEnumerable<object> objects)
        {
            foreach (var item in objects)
            {
                item.LogOut();
            }
        }
        public static void LogOut(this object objects)
        {
            var jsonString = JsonConvert.SerializeObject(objects);
            jsonString = jsonString.Replace("{", "{\n");
            jsonString = jsonString.Replace(",", ",\n");
            jsonString = jsonString.Replace("}", "\n}");
            Console.WriteLine(jsonString);
        }
        public static string MD5Hash(this string password)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
