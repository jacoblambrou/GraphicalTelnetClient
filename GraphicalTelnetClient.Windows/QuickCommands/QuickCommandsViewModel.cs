using GraphicalTelnetClient.Windows.Settings;
using GraphicalTelnetClient.Windows.TelnetViewer;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows.QuickCommands
{
    public struct DeviceInfo
    {
        public DeviceInfo(string type, string code)
        {
            Type = type;
            Code = code;
        }

        public string Type { get; init; }
        public string Code { get; init; }
    };
    

    public class QuickCommandsViewModel : BindableBase
    {
        public DelegateCommand StationStatusCommand { get; private set; }
        public DelegateCommand TrunkStatusCommand { get; private set; }

        private TelnetViewerViewModel telnetViewerViewModel;

        public QuickCommandsViewModel(SettingsBindableModel defaultSettings, TelnetViewerViewModel telnetViewerViewModel)
        {
            SetStationHelpTooltip();
            SetTrunkHelpTooltip();

            StationStatusCommand = new DelegateCommand(OnStationStatusCommand, CanSendStationStatusCommand);
            TrunkStatusCommand = new DelegateCommand(OnTrunkStatusCommand, CanSendTrunkStatusCommand);

            this.telnetViewerViewModel = telnetViewerViewModel;

            IsExpanded = defaultSettings.AutomaticallyExpandQuickCommands;

            SetStationStatusTool();
            SetTrunkStatusTool();
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private DeviceInfo[] _stationStatusTooltip;
        public DeviceInfo[] StationStatusTooltip
        {
            get { return _stationStatusTooltip; }
            set { SetProperty(ref _stationStatusTooltip, value); }
        }

        private DeviceInfo[] _trunkStatusTooltip;
        public DeviceInfo[] TrunkStatusTooltip
        {
            get { return _trunkStatusTooltip; }
            set { SetProperty(ref _trunkStatusTooltip, value); }
        }

        private ValidatablePortStatus _stationPorts;
        public ValidatablePortStatus StationPorts
        {
            get { return _stationPorts; }
            set { SetProperty(ref _stationPorts, value); }
        }

        private ValidatablePortStatus _trunkPorts;
        public ValidatablePortStatus TrunkPorts
        {
            get { return _trunkPorts; }
            set { SetProperty(ref _trunkPorts, value); }
        }

        private void OnStationStatusCommand()
        {
            string stationStartHex = int.Parse(StationPorts.StartPort).ToString("X");
            string stationEndHex = int.Parse(StationPorts.EndPort).ToString("X");
            
            stationStartHex = EnsureFourDigitHexString(stationStartHex);
            stationEndHex= EnsureFourDigitHexString(stationEndHex);

            this.telnetViewerViewModel.SendQuickCommand($"status sta {stationStartHex} {stationEndHex}");
        }

        private void OnTrunkStatusCommand()
        {
            string trunkStartHex = int.Parse(TrunkPorts.StartPort).ToString("X");
            string trunkEndHex = int.Parse(TrunkPorts.EndPort).ToString("X");

            trunkStartHex = EnsureFourDigitHexString(trunkStartHex);
            trunkEndHex = EnsureFourDigitHexString(trunkEndHex);

            this.telnetViewerViewModel.SendQuickCommand($"status trk {trunkStartHex} {trunkEndHex}");
        }

        private void SetStationHelpTooltip()
        {
            var stationStatusHelpVariables = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["StationTypes"]).Split(',');

            StationStatusTooltip = new DeviceInfo[stationStatusHelpVariables.Length];

            for (int i = 0; i < stationStatusHelpVariables.Length; i++)
            {
                var stationStatus = stationStatusHelpVariables[i].Split('-');
                StationStatusTooltip[i] = new(stationStatus[0], stationStatus[1]);
            }
        }

        private void SetTrunkHelpTooltip()
        {
            var trunkStatusHelpVariables = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["TrunkTypes"]).Split(',');

            TrunkStatusTooltip = new DeviceInfo[trunkStatusHelpVariables.Length];

            for (int i = 0; i < trunkStatusHelpVariables.Length; i++)
            {
                var trunkStatus = trunkStatusHelpVariables[i].Split('-');
                TrunkStatusTooltip[i] = new(trunkStatus[0], trunkStatus[1]);
            }
        }

        public void SetStationStatusTool()
        {
            if (StationPorts != null)
                StationPorts.ErrorsChanged -= StationRaiseCanExecuteChanged;

            StationPorts = new ValidatablePortStatus();

            StationPorts.ErrorsChanged += StationRaiseCanExecuteChanged;

            StationPorts.StartPort = "0001";
            StationPorts.EndPort = "0001";
        }

        public void SetTrunkStatusTool()
        {
            if (TrunkPorts != null)
                TrunkPorts.ErrorsChanged -= TrunkRaiseCanExecuteChanged;

            TrunkPorts = new ValidatablePortStatus();

            TrunkPorts.ErrorsChanged += TrunkRaiseCanExecuteChanged;

            TrunkPorts.StartPort = "0001";
            TrunkPorts.EndPort = "0001";
        }

        private bool CanSendStationStatusCommand()
        {
            return !StationPorts.HasErrors;
        }

        private bool CanSendTrunkStatusCommand()
        {
            return !TrunkPorts.HasErrors;
        }

        private string EnsureFourDigitHexString(string hexString)
        {
            while (hexString.Length < 4)
                hexString = $"0{hexString}";
            return hexString;
        }

        private void StationRaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e) =>
            StationStatusCommand.RaiseCanExecuteChanged();

        private void TrunkRaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e) =>
            TrunkStatusCommand.RaiseCanExecuteChanged();
    }
}
