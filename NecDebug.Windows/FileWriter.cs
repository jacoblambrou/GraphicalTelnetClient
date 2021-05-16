using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecDebug.Windows
{
    public class FileWriter
    {
        public static async Task WriteToFile(string file, string text)
        {
            using StreamWriter streamWriter = new(file, append: true);
            await streamWriter.WriteAsync(text);
        }
    }
}
