﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecDebug.Windows.Settings
{
    public class SettingsModel : BindableBase
    {
        private string _defaultServerAddress;
        public string DefaultServerAddress
        {
            get { return _defaultServerAddress; }
            set { SetProperty(ref _defaultServerAddress, value); }
        }

        private int _defaultServerPort;
        public int DefaultServerPort
        {
            get { return _defaultServerPort; }
            set { SetProperty(ref _defaultServerPort, value); }
        }

        private string _defaultOutputDirectory;
        public string DefaultOutputDirectory
        {
            get { return _defaultOutputDirectory; }
            set { SetProperty(ref _defaultOutputDirectory, value); }
        }

        private string _defaultFileName;
        public string DefaultFileName
        {
            get { return _defaultFileName; }
            set { SetProperty(ref _defaultFileName, value); }
        }

        private bool _automaticallySaveToFile;
        public bool AutomaticallySaveToFile
        {
            get { return _automaticallySaveToFile; }
            set { SetProperty(ref _automaticallySaveToFile, value); }
        }
    }
}
