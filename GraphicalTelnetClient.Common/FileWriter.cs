using System.IO;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Common
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
