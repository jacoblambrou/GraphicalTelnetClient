using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GraphicalTelnetClient.Common.Xml
{
    public class XmlReader
    {
        public UserSettingsModel ObtainUserDetailsFromXmlFile(string filePath, UserSettingsModel userSettingsModel)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            userSettingsModel.DefaultServerAddress = xmlDoc.GetElementsByTagName("DefaultServerAddress")[0].InnerText;
            userSettingsModel.DefaultServerPort= int.Parse(xmlDoc.GetElementsByTagName("DefaultServerPort")[0].InnerText);
            userSettingsModel.DefaultOutputDirectory = Environment.ExpandEnvironmentVariables(xmlDoc.GetElementsByTagName("DefaultOutputDirectory")[0].InnerText);
            userSettingsModel.DefaultFileName = xmlDoc.GetElementsByTagName("DefaultFileName")[0].InnerText;
            userSettingsModel.AutomaticallySaveToFile = bool.Parse(xmlDoc.GetElementsByTagName("AutomaticallySaveToFile")[0].InnerText);

            return userSettingsModel;
        }
    }
}
