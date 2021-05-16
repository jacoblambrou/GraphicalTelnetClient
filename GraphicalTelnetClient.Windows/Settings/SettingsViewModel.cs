using GraphicalTelnetClient.Common;
using GraphicalTelnetClient.Common.Xml;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Configuration;
using System.IO;

namespace GraphicalTelnetClient.Windows.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private UserSettingsModel userSettings;
        private string filePath;
        private XmlCreator xmlCreator;
        private XmlReader xmlReader;

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand BrowseCommand { get; private set; }

        public SettingsViewModel()
        {
            SaveCommand = new DelegateCommand(OnSaveCommand);
            BrowseCommand = new DelegateCommand(OnBrowseCommand);

            userSettings = new UserSettingsModel();
            xmlCreator = new XmlCreator();

            filePath = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["UserSettingsFilePath"]);
            int directoryIndex = filePath.LastIndexOf(@"\");

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, directoryIndex));
                xmlCreator.CreateDefaultUserSettingsXmlFile(filePath);
            }

            xmlReader = new XmlReader();

            xmlReader.ObtainUserDetailsFromXmlFile(filePath, userSettings);

            DefaultSettings = new SettingsBindableModel
            {
                DefaultServerAddress = userSettings.DefaultServerAddress,
                DefaultServerPort = userSettings.DefaultServerPort,
                DefaultOutputDirectory = userSettings.DefaultOutputDirectory,
                DefaultFileName = userSettings.DefaultFileName,
                AutomaticallySaveToFile = userSettings.AutomaticallySaveToFile,
            };

            LastSaved = "Never.";
        }

        private SettingsBindableModel _defaultSettings;
        public SettingsBindableModel DefaultSettings
        {
            get { return _defaultSettings; }
            set { SetProperty(ref _defaultSettings, value); }
        }

        private string _lastSaved;
        public string LastSaved
        {
            get { return _lastSaved; }
            set { SetProperty(ref _lastSaved, value); }
        }

        private void OnBrowseCommand()
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();

            if ((bool)dialog.ShowDialog())
                DefaultSettings.DefaultOutputDirectory = dialog.SelectedPath;
        }

        private void OnSaveCommand()
        {
            userSettings.DefaultServerAddress = DefaultSettings.DefaultServerAddress;
            userSettings.DefaultServerPort = DefaultSettings.DefaultServerPort;
            userSettings.DefaultOutputDirectory = DefaultSettings.DefaultOutputDirectory;
            userSettings.DefaultFileName = DefaultSettings.DefaultFileName;
            userSettings.AutomaticallySaveToFile = DefaultSettings.AutomaticallySaveToFile;

            xmlCreator.UpdateUserSettingsXmlFile(filePath, userSettings);

            LastSaved = DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss");
        }
    }
}