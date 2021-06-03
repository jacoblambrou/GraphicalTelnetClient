using GraphicalTelnetClient.Common;
using GraphicalTelnetClient.Windows.ConnectionDetails;
using GraphicalTelnetClient.Windows.QuickCommands;
using GraphicalTelnetClient.Windows.Settings;
using GraphicalTelnetClient.Windows.TelnetViewer;
using Prism.Commands;
using Prism.Mvvm;

namespace GraphicalTelnetClient.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        private ConnectionDetailsViewModel connectionDetailsViewModel;
        private TelnetViewerViewModel telnetViewerViewModel;
        private SettingsViewModel settingsViewModel;
        private QuickCommandsViewModel quickCommandsViewModel;
        private FileWriter fileWriter;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand OpenSettingsCommand { get; private set; }
        public DelegateCommand ExitSettingsCommand { get; private set; }

        public MainWindowViewModel()
        {
            LoadedCommand = new DelegateCommand(OnLoaded);
            OpenSettingsCommand = new DelegateCommand(OnOpenSettingsCommand);
            ExitSettingsCommand = new DelegateCommand(OnExitSettingsCommand);

            fileWriter = new FileWriter();

            this.settingsViewModel = new SettingsViewModel();
            this.telnetViewerViewModel = new TelnetViewerViewModel(fileWriter);
            this.connectionDetailsViewModel = new ConnectionDetailsViewModel(this.settingsViewModel.DefaultSettings, this.telnetViewerViewModel, fileWriter);
            this.quickCommandsViewModel = new QuickCommandsViewModel(this.settingsViewModel.DefaultSettings, this.telnetViewerViewModel);
        }

        private BindableBase _connectionDetailsViewModel;
        public BindableBase ConnectionDetailsViewModel
        {
            get { return _connectionDetailsViewModel; }
            set { SetProperty(ref _connectionDetailsViewModel, value); }
        }

        private BindableBase _mainViewModel;
        public BindableBase MainViewModel
        {
            get { return _mainViewModel; }
            set { SetProperty(ref _mainViewModel, value); }
        }

        private BindableBase _quickCommandsViewModel;
        public BindableBase QuickCommandsViewModel
        {
            get { return _quickCommandsViewModel; }
            set { SetProperty(ref _quickCommandsViewModel, value); }
        }

        private bool _settingsOpen;
        public bool SettingsOpen
        {
            get { return _settingsOpen; }
            set { SetProperty(ref _settingsOpen, value); }
        }

        private void OnLoaded()
        {
            this.ConnectionDetailsViewModel = connectionDetailsViewModel;
            this.MainViewModel = telnetViewerViewModel;
            this.QuickCommandsViewModel = quickCommandsViewModel;
        }

        private void OnOpenSettingsCommand()
        {
            this.MainViewModel = settingsViewModel;
            SettingsOpen = true;
        }

        private void OnExitSettingsCommand()
        {
            this.MainViewModel = telnetViewerViewModel;
            SettingsOpen = false;
        }
    }
}
