using GraphicalTelnetClient.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace GraphicalTelnetClient.Windows.TelnetViewer
{
    public class TelnetViewerViewModel : BindableBase
    {
        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand<string> ScrollHistoryCommand { get; private set; }
        public DelegateCommand ClearUserInputCommand { get; private set; }

        private FileWriter FileWriter;

        private List<string> commandHistory;
        private int historyCount;
        private int historyIndex;

        public TelnetClient TelnetClient;

        public TelnetViewerViewModel(FileWriter fileWriter)
        {
            SendCommand = new DelegateCommand(OnSendCommand);
            ScrollHistoryCommand = new DelegateCommand<string>(OnScrollHistoryCommand);
            ClearUserInputCommand = new DelegateCommand(OnClearUserInputCommand);

            FileWriter = fileWriter;
            FileWriter.ExceptionOccurred += FileWriter_ExceptionOccurred;

            TelnetClient = new TelnetClient();
            TelnetClient.ResponseReceived += TelnetClient_ResponseReceived;

            AlwaysScrollToEnd = true;

            commandHistory = new List<string>();
        }

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

        private int _selectionStart;
        public int SelectionStart
        {
            get { return _selectionStart; }
            set { SetProperty(ref _selectionStart, value); }
        }

        private int _selectionLength;
        public int SelectionLength
        {
            get { return _selectionLength; }
            set { SetProperty(ref _selectionLength, value); }
        }

        private bool _focusOutput;
        public bool FocusOutput
        {
            get { return _focusOutput; }
            set { SetProperty(ref _focusOutput, value); }
        }

        private bool _focusUserInput;
        public bool FocusUserInput
        {
            get { return _focusUserInput; }
            set { SetProperty(ref _focusUserInput, value); }
        }

        private bool _alwaysScrollToEnd;
        public bool AlwaysScrollToEnd
        {
            get { return _alwaysScrollToEnd; }
            set { SetProperty(ref _alwaysScrollToEnd, value); }
        }

        private void OnSendCommand()
        {
            commandHistory.Add(UserInput);
            TelnetClient.Send(UserInput);
            UserInput = string.Empty;
        }

        public void SendQuickCommand(string command)
        {
            commandHistory.Add(command);
            TelnetClient.Send(command);
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

        public void ClearOutput() =>
            Output = string.Empty;

        public void SearchOutput(string searchText)
        {
            SelectionStart = 0;
            SelectionLength = 0;
            
            FocusOutput = false;
            
            SelectionStart = Output.IndexOf(searchText);
            SelectionLength = searchText.Length;

            FocusOutput = true;
        }

        private void FileWriter_ExceptionOccurred(string exMessage) =>
            AppendToOutput(exMessage);

        private void TelnetClient_ResponseReceived(string response) =>
            AppendToOutput(response);

        private void AppendToOutput(string update) =>
            Output += $"{update}";

        public void ScrollToEnd()
        {
            AlwaysScrollToEnd = false;
            AlwaysScrollToEnd = true;
        }
    }
}
