using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XUnitDemo.Models;

namespace XUnitDemo.Extensions
{
    public class ConvertJsonToModel
    {
        // / = \\
        public const string UserFilePath = "D:/Users/ACER/Source/Consoles/XUnitDemo/Data/UserData.json";
        public static List<T> ConvertUsersSync<T>()
        {
            using (StreamReader r = new StreamReader(UserFilePath))
            {
                string json = r.ReadToEnd();
                var contents = JsonConvert.DeserializeObject<List<T>>(json);
                return contents;
            }
        }
    }
}
