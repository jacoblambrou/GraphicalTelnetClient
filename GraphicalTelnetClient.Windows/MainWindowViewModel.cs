using GraphicalTelnetClient.Windows.ConnectionDetails;
using GraphicalTelnetClient.Windows.Settings;
using GraphicalTelnetClient.Windows.TelnetViewer;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        private ConnectionDetailsViewModel connectionDetailsViewModel;
        private TelnetViewerViewModel telnetViewerViewModel;
        private SettingsViewModel settingsViewModel;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand OpenSettingsCommand { get; private set; }
        public DelegateCommand ExitSettingsCommand { get; private set; }

        public MainWindowViewModel()
        {
            LoadedCommand = new DelegateCommand(OnLoaded);
            OpenSettingsCommand = new DelegateCommand(OnOpenSettingsCommand);
            ExitSettingsCommand = new DelegateCommand(OnExitSettingsCommand);

            this.settingsViewModel = new SettingsViewModel();
            this.telnetViewerViewModel = new TelnetViewerViewModel();
            this.connectionDetailsViewModel = new ConnectionDetailsViewModel(settingsViewModel.DefaultSettings, this.telnetViewerViewModel);
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
