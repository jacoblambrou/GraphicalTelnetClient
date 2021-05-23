using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Common
{
    public interface ILogger
    {
        public event Action<string> ExceptionOccurred;

        public void OnExceptionOccurred(Exception ex);
    }
}
