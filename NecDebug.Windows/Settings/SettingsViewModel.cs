using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecDebug.Windows.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private SettingsModel _defaultSettings;

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }

        public SettingsViewModel()
        {
            SaveCommand = new DelegateCommand(OnSaveCommand);
            BrowseCommand = new DelegateCommand(OnBrowseCommand);

            DefaultSettings = new SettingsModel
            {
                DefaultServerAddress = ConfigurationManager.AppSettings["DefaultServerAddress"],
                DefaultServerPort = int.Parse(ConfigurationManager.AppSettings["DefaultServerPort"]),
                DefaultOutputDirectory = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["DefaultOutputDirectory"]),
                DefaultFileName = ConfigurationManager.AppSettings["DefaultFileName"],
                AutomaticallySaveToFile = bool.Parse(ConfigurationManager.AppSettings["AutomaticallySaveToFile"]),
            };
        }

        public SettingsModel DefaultSettings
        {
            get { return _defaultSettings; }
            set { SetProperty(ref _defaultSettings, value); }
        }

        private void OnBrowseCommand()
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();

            if ((bool)dialog.ShowDialog())
                DefaultSettings.DefaultOutputDirectory = dialog.SelectedPath;
        }

        private void OnSaveCommand()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["DefaultServerAddress"].Value = DefaultSettings.DefaultServerAddress;
            config.AppSettings.Settings["DefaultServerPort"].Value = DefaultSettings.DefaultServerPort.ToString();
            config.AppSettings.Settings["DefaultOutputDirectory"].Value = DefaultSettings.DefaultOutputDirectory;
            config.AppSettings.Settings["DefaultFileName"].Value = DefaultSettings.DefaultFileName;
            config.AppSettings.Settings["AutomaticallySaveToFile"].Value = DefaultSettings.AutomaticallySaveToFile.ToString();
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}