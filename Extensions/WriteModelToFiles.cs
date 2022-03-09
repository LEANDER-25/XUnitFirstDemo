using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitDemo.Models;

namespace XUnitDemo.Extensions
{
    public class WriteModelToFiles
    {
        public static async Task AppendToExisted<T>(T obj, string path)
        {
            List<T> reads = ConvertJsonToModel.ConvertUsersSync<T>();
            using (StreamWriter file = new(path))
            {
                if (reads == null || reads.Count == 0)
                {
                    reads = new List<T>();
                }
                else
                {
                    reads.Add(obj);
                }
                await file.WriteLineAsync("[");
                foreach (var item in reads)
                {
                    var content = JsonConvert.SerializeObject(item) + ",";
                    await file.WriteLineAsync(content);
                }
                await file.WriteLineAsync("]");
            }
        }
    }
}
