using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NecDebug.Windows
{
    enum TelnetCommands
    {
        WILL = 0xFB,
        WONT = 0xFC,
        DO = 0xFD,
        DONT = 0xFE,
        IAC = 0xFF,
        Echo = 0x01,
        Suppress = 0x03
    }

    public class TelnetClient
    {
        public event EventHandler<bool> ConnectionStatusChanged;
        public event Action<string> ResponseReceived;

        TcpClient tcpClient;
        NetworkStream netStream;
        Byte[] buffer = Encoding.UTF8.GetBytes("");
        string responseData = string.Empty;
        Task readDataTask;
        CancellationTokenSource cancellationTokenSource = new();

        public async Task ConnectToServer(string serverAddress, int serverPort)
        {
            try
            {
                tcpClient = new TcpClient();

                await tcpClient.ConnectAsync(serverAddress, serverPort, cancellationTokenSource.Token);

                OnConnectionStatusChanged(tcpClient.Connected);

                netStream = tcpClient.GetStream();

                buffer = new byte[1024];

                StringBuilder sb = new();

                readDataTask = Task.Run(() => ReadData(netStream, sb));
            
                await readDataTask;
            }
            catch (Exception ex)
            {
                OnResponseReceived($"{Environment.NewLine}Unable to connect.{Environment.NewLine}");
            }
        }

        public void DisconnectFromServer()
        {
            tcpClient.Close();
            OnConnectionStatusChanged(false);
        }

        private void ReadData(NetworkStream netStream, StringBuilder sb)
        {
            while (true)
            {
                do
                {
                    try
                    {
                        var bytes = netStream.Read(buffer, 0, buffer.Length);
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                    }
                    catch (Exception ex)
                    {
                        sb.Append($"{Environment.NewLine}Connection lost.{Environment.NewLine}");
                        OnResponseReceived(sb.ToString());
                        return;
                    }
                } while (netStream.DataAvailable);

                responseData = sb.ToString();
                
                OnResponseReceived(responseData);
                sb.Clear();
            };
        }

        internal void Send(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes($"{input}{Environment.NewLine}");
            try
            {
                netStream.Write(inputBytes, 0, inputBytes.Length);
            }
            catch (Exception ex)
            {
                OnResponseReceived($"{Environment.NewLine}Send failed. Please check connection.{Environment.NewLine}");
            }
        }

        public void OnConnectionStatusChanged(bool IsConnected)
        {
            ConnectionStatusChanged?.Invoke(this, IsConnected);
        }

        public void OnResponseReceived(string response)
        {
            ResponseReceived?.Invoke(response);
        }
    }
}
