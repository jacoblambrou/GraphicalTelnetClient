using System.Xml;

namespace GraphicalTelnetClient.Common.Xml
{
    public class XmlCreator
    {
        private static XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            NewLineOnAttributes = true,
            Indent = true,
            IndentChars = "\t",
        };

        public void CreateDefaultUserSettingsXmlFile(string filePath)
        {
            using (var xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("UserSettings");
                xmlWriter.WriteStartElement("ServerConnection");
                xmlWriter.WriteElementString("DefaultServerAddress", "");
                xmlWriter.WriteElementString("DefaultServerPort", "5964");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("OutputFile");
                xmlWriter.WriteElementString("DefaultOutputDirectory", "%userprofile%\\Downloads");
                xmlWriter.WriteElementString("DefaultFileName", "TelnetOutput");
                xmlWriter.WriteElementString("AutomaticallySaveToFile", "true");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Layout");
                xmlWriter.WriteElementString("AutomaticallyExpandQuickCommands", "true");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        public void UpdateUserSettingsXmlFile(string filePath, UserSettingsModel userSettings)
        {
            using (var xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("UserSettings");
                xmlWriter.WriteStartElement("ServerConnection");
                xmlWriter.WriteElementString("DefaultServerAddress", userSettings.DefaultServerAddress);
                xmlWriter.WriteElementString("DefaultServerPort", userSettings.DefaultServerPort.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("OutputFile");
                xmlWriter.WriteElementString("DefaultOutputDirectory", userSettings.DefaultOutputDirectory);
                xmlWriter.WriteElementString("DefaultFileName", userSettings.DefaultFileName);
                xmlWriter.WriteElementString("AutomaticallySaveToFile", userSettings.AutomaticallySaveToFile.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Layout");
                xmlWriter.WriteElementString("AutomaticallyExpandQuickCommands", userSettings.AutomaticallyExpandQuickCommands.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
    }
}
