using GraphicalTelnetClient.Windows.Settings;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows.TelnetViewer
{
    public class TelnetViewerViewModel : BindableBase
    {
        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand<string> ScrollHistoryCommand { get; private set; }
        public DelegateCommand ClearUserInputCommand { get; private set; }

        public TelnetClient TelnetClient;
        private List<string> commandHistory;
        private int historyCount;
        private int historyIndex;

        public TelnetViewerViewModel()
        {
            SendCommand = new DelegateCommand(OnSendCommand);
            ScrollHistoryCommand = new DelegateCommand<string>(OnScrollHistoryCommand);
            ClearUserInputCommand = new DelegateCommand(OnClearUserInputCommand);

            TelnetClient = new TelnetClient();
            TelnetClient.ResponseReceived += TelnetClient_ResponseReceived;


            commandHistory = new List<string>();
        }

        private bool echoOn;        //TODO: Identify whether echo is turned on at the server end. If it is, stop printing user input to output.

        private string _userInput;
        public string UserInput
        {
            get { return _userInput; }
            set { SetProperty(ref _userInput, value); }
        }

        private string _output;
        public string Output
        {
            get { return _output; }
            set { SetProperty(ref _output, value); }
        }

        private bool _focusUserInput;
        public bool FocusUserInput
        {
            get { return _focusUserInput; }
            set { SetProperty(ref _focusUserInput, value); }
        }


        private void OnSendCommand()
        {
            commandHistory.Add(UserInput);
            TelnetClient.Send(UserInput);
            UserInput = string.Empty;
        }

        private void OnScrollHistoryCommand(string direction)
        {
            // Ensure history isn't empty
            if (commandHistory.Count == 0)
                return;

            // Check to see whether any new commands have been used, if they have reset historyIndex to last command
            if (historyCount != commandHistory.Count)
            {
                historyCount = commandHistory.Count;

                historyIndex = commandHistory.Count;

                EnumerateHistory(direction);
            }
            else
            {
                EnumerateHistory(direction);
            }
        }

        private void OnClearUserInputCommand()
        {
            UserInput = string.Empty;

            // Reset historyIndex to last command
            historyIndex = commandHistory.Count;
        }

        private void EnumerateHistory(string direction)
        {
            if (direction == "up")
            {
                // Ensure historyIndex doesn't go below 0
                if (historyIndex > 0)
                    historyIndex--;
                UserInput = commandHistory[historyIndex];
            }
            else
            {
                // Ensure historyIndex doesn't go out of range while historyCount is 1
                if (historyCount == 1)
                    UserInput = commandHistory[0];

                // Ensure historyIndex doesn't go out of range
                else if (historyIndex < historyCount - 1)
                {
                    historyIndex++;
                    UserInput = commandHistory[historyIndex];
                }
            }
        }

        public void ClearOutput()
        {
            Output = string.Empty;
        }

        private void TelnetClient_ResponseReceived(string response)
        {
            AppendToOutput(response);
        }

        private void AppendToOutput(string update)
        {
            Output += $"{update}";
        }
    }
}
