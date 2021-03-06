using GraphicalTelnetClient.Common;
using GraphicalTelnetClient.Windows.Settings;
using GraphicalTelnetClient.Windows.TelnetViewer;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.ComponentModel;

namespace GraphicalTelnetClient.Windows.ConnectionDetails
{
    public class ConnectionDetailsViewModel : BindableBase
    {
        private TelnetViewerViewModel telnetViewerViewModel;
        private string dateTime;
        private FileWriter fileWriter;

        public DelegateCommand ConnectDisconnectCommand { get; private set; }
        public DelegateCommand DisconnectCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }
        public DelegateCommand ScrollToEndCommand { get; private set; }

        public ConnectionDetailsViewModel(SettingsBindableModel defaultSettings, TelnetViewerViewModel telnetViewerViewModel, FileWriter fileWriter)
        {
            this.telnetViewerViewModel = telnetViewerViewModel;
            this.fileWriter = fileWriter;

            ConnectDisconnectCommand = new DelegateCommand(OnConnectDisconnectCommand, CanConnectDisconnect);
            ClearCommand = new DelegateCommand(OnClearCommand);
            BrowseCommand = new DelegateCommand(OnBrowseCommand);
            ScrollToEndCommand = new DelegateCommand(OnScrollToEndCommand);

            telnetViewerViewModel.TelnetClient.ConnectionStatusChanged += DisconnectCommandRaiseCanExecuteChanged;
            telnetViewerViewModel.TelnetClient.ResponseReceived += TelnetClient_ResponseReceived;

            SetDefaultConnectionDetails();

            ConnectionDetails.ServerAddress = defaultSettings.DefaultServerAddress;
            ConnectionDetails.ServerPort = defaultSettings.DefaultServerPort;
            OutputDirectory = defaultSettings.DefaultOutputDirectory;
            FileName = defaultSettings.DefaultFileName;
            IsSavingToFile = defaultSettings.AutomaticallySaveToFile;
        }

        private ValidatableConnectionDetailsModel _connectionDetails;
        public ValidatableConnectionDetailsModel ConnectionDetails
        {
            get { return _connectionDetails; }
            set { SetProperty(ref _connectionDetails, value); }
        }

        private bool _isSavingToFile;
        public bool IsSavingToFile
        {
            get { return _isSavingToFile; }
            set { SetProperty(ref _isSavingToFile, value); }
        }

        private string _outputDirectory;
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set { SetProperty(ref _outputDirectory, value); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }


        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                SetProperty(ref _isConnected, value);
                telnetViewerViewModel.FocusUserInput = IsConnected;
            }
        }

        private bool CanConnectDisconnect()
        {
            if (!ConnectionDetails.HasErrors && !IsConnected)
                return true;
            else if (IsConnected)
                return true;
            else
                return false;
        }

        private async void OnConnectDisconnectCommand()
        {
            dateTime = DateTime.Now.ToString("ddMMyyyy_HHmm");

            if (!IsConnected)
            {
                if (IsSavingToFile)
                    fileWriter.SetOutputFile($"{OutputDirectory}\\{FileName}_{dateTime}.txt");

                telnetViewerViewModel.Output += $"{Environment.NewLine}Connecting...";

                await telnetViewerViewModel.TelnetClient.ConnectToServer(ConnectionDetails.ServerAddress, ConnectionDetails.ServerPort);
            }
            else
            {
                telnetViewerViewModel.TelnetClient.DisconnectFromServer();
            }
        }

        private void OnClearCommand()
        {
            telnetViewerViewModel.ClearOutput();
            telnetViewerViewModel.FocusUserInput = false;
            telnetViewerViewModel.FocusUserInput = true;
        }

        private void OnBrowseCommand()
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();

            if ((bool)dialog.ShowDialog())
                OutputDirectory = dialog.SelectedPath;
        }

        private void OnScrollToEndCommand() =>
            telnetViewerViewModel.ScrollToEnd();

        private void SetDefaultConnectionDetails()
        {
            if (ConnectionDetails != null)
                ConnectionDetails.ErrorsChanged -= ConnectCommandRaiseCanExecuteChanged;

            ConnectionDetails = new ValidatableConnectionDetailsModel();
            ConnectionDetails.ErrorsChanged += ConnectCommandRaiseCanExecuteChanged;

            ConnectionDetails.ServerAddress = string.Empty;
            ConnectionDetails.ServerPort = 5964;
        }

        private void ConnectCommandRaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ConnectDisconnectCommand.RaiseCanExecuteChanged();
        }

        private void DisconnectCommandRaiseCanExecuteChanged(object sender, bool isConnected)
        {
            ConnectDisconnectCommand.RaiseCanExecuteChanged();
            IsConnected = isConnected;
        }

        private async void TelnetClient_ResponseReceived(string response)
        {
            if (!IsConnected)
                return;

            if (!IsSavingToFile)
                return;

            if (string.IsNullOrWhiteSpace(OutputDirectory) || string.IsNullOrWhiteSpace(FileName))
                return;

            await fileWriter.WriteToFile(response);
        }
    }
}
