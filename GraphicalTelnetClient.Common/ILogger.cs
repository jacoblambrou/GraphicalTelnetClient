using System;

namespace GraphicalTelnetClient.Common
{
    public interface ILogger
    {
        public event Action<string> ExceptionOccurred;

        public void OnExceptionOccurred(Exception ex);
    }
}
