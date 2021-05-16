using NecDebug.Windows.ConnectionDetails;
using NecDebug.Windows.Settings;
using NecDebug.Windows.TelnetViewer;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecDebug.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        private ConnectionDetailsViewModel connectionDetailsViewModel;
        private TelnetViewerViewModel telnetViewerViewModel;
        private SettingsViewModel settingsViewModel;

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }

        public MainWindowViewModel()
        {
            LoadedCommand = new DelegateCommand(OnLoaded);
            SettingsCommand = new DelegateCommand(OnSettingsCommand);

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

        private void OnLoaded()
        {
            this.ConnectionDetailsViewModel = connectionDetailsViewModel;
            this.MainViewModel = telnetViewerViewModel;
        }

        private void OnSettingsCommand()
        {
            this.MainViewModel = settingsViewModel;
        }
    }
}
