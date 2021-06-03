namespace GraphicalTelnetClient.Common
{
    public class UserSettingsModel
    {
        public string DefaultServerAddress { get; set; }
        public int DefaultServerPort { get; set; }
        public string DefaultOutputDirectory { get; set; }
        public string DefaultFileName { get; set; }
        public bool AutomaticallySaveToFile { get; set; }
        public bool AutomaticallyExpandQuickCommands { get; set; }
    }
}
