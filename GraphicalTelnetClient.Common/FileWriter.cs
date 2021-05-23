using System;
using System.IO;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Common
{
    public class FileWriter : ILogger
    {
        public event Action<string> ExceptionOccurred;

        private StreamWriter streamWriter;
        private string filePath;

        public async Task WriteToFile(string text)
        {
            try
            {
                using StreamWriter streamWriter = new StreamWriter(filePath, append: true);
                await streamWriter.WriteAsync(text);
            }
            catch (Exception ex)
            {
                OnExceptionOccurred(ex);
            }
        }

        public void SetOutputFile(string filePath) =>
            this.filePath = filePath;

        public void OnExceptionOccurred(Exception ex) =>
            ExceptionOccurred?.Invoke(ex.Message);
    }
}
