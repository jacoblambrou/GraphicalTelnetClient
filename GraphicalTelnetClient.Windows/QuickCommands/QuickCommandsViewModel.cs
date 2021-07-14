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
        public DelegateCommand RebootCommand { get; private set; }
        public DelegateCommand CheckUptimeCommand { get; private set; }
        public DelegateCommand SlotInfoCommand { get; private set; }

        private TelnetViewerViewModel telnetViewerViewModel;

        public QuickCommandsViewModel(SettingsBindableModel defaultSettings, TelnetViewerViewModel telnetViewerViewModel)
        {
            SetStationHelpTooltip();
            SetTrunkHelpTooltip();

            StationStatusCommand = new DelegateCommand(OnStationStatusCommand, CanSendStationStatusCommand);
            TrunkStatusCommand = new DelegateCommand(OnTrunkStatusCommand, CanSendTrunkStatusCommand);
            RebootCommand = new DelegateCommand(OnRebootCommand);
            CheckUptimeCommand = new DelegateCommand(OnCheckUptimeCommand);
            SlotInfoCommand = new DelegateCommand(OnSlotInfoCommand, CanSendSlotInfoCommand);

            this.telnetViewerViewModel = telnetViewerViewModel;

            IsExpanded = defaultSettings.AutomaticallyExpandQuickCommands;

            SetStationStatusTool();
            SetTrunkStatusTool();
            SetSlotInfoTool();
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

        private ValidatableSlotNumber _slotNumber;
        public ValidatableSlotNumber SlotNumber
        {
            get { return _slotNumber; }
            set { SetProperty(ref _slotNumber, value); }
        }


        private void OnStationStatusCommand() =>
            this.telnetViewerViewModel.SendQuickCommand($"status sta {StationPorts.StartHexPort} {StationPorts.EndHexPort}");


        private void OnTrunkStatusCommand() =>
            this.telnetViewerViewModel.SendQuickCommand($"status trk {TrunkPorts.StartHexPort} {TrunkPorts.EndHexPort}");

        private void OnRebootCommand() =>
            this.telnetViewerViewModel.SendQuickCommand("reset");

        private void OnCheckUptimeCommand() =>
            this.telnetViewerViewModel.SendQuickCommand("date");

        private void OnSlotInfoCommand() =>
            this.telnetViewerViewModel.SendQuickCommand($"slot info {EnsureTwoDigitValue(SlotNumber.SlotNumber)}");

        private void SetStationHelpTooltip() =>
            StationStatusTooltip = SetHelpTooltip("StationTypes");

        private void SetTrunkHelpTooltip() =>
            TrunkStatusTooltip = SetHelpTooltip("TrunkTypes");

        private DeviceInfo[] SetHelpTooltip(string type)
        {
            var statusHelpVariables = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings[type]).Split(',');

            var deviceInfo = new DeviceInfo[statusHelpVariables.Length];

            for (int i = 0; i < statusHelpVariables.Length; i++)
            {
                var helpVariable = statusHelpVariables[i].Split(':');
                deviceInfo[i] = new(helpVariable[0], helpVariable[1]);
            }

            return deviceInfo;
        }

        private void SetStationStatusTool()
        {
            if (StationPorts != null)
                StationPorts.ErrorsChanged -= StationRaiseCanExecuteChanged;

            StationPorts = new ValidatablePortStatus();

            StationPorts.ErrorsChanged += StationRaiseCanExecuteChanged;

            StationPorts.StartPort = "0001";
            StationPorts.EndPort = "0001";

        }

        private void SetTrunkStatusTool()
        {
            if (TrunkPorts != null)
                TrunkPorts.ErrorsChanged -= TrunkRaiseCanExecuteChanged;

            TrunkPorts = new ValidatablePortStatus();

            TrunkPorts.ErrorsChanged += TrunkRaiseCanExecuteChanged;

            TrunkPorts.StartPort = "0001";
            TrunkPorts.EndPort = "0001";
        }

        private void SetSlotInfoTool()
        {
            if (SlotNumber != null)
                SlotNumber.ErrorsChanged -= SlotRaiseCanExcuteCHanged;

            SlotNumber = new ValidatableSlotNumber();
            SlotNumber.ErrorsChanged += SlotRaiseCanExcuteCHanged;
            SlotNumber.SlotNumber = "01";
        }

        private bool CanSendStationStatusCommand()
        {
            return !StationPorts.HasErrors;
        }

        private bool CanSendTrunkStatusCommand()
        {
            return !TrunkPorts.HasErrors;
        }

        private bool CanSendSlotInfoCommand()
        {
            return !SlotNumber.HasErrors;
        }

        private string EnsureTwoDigitValue(string value)
        {
            while (value.Length < 2)
                value = $"0{value}";
            return value;
        }

        private void StationRaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e) =>
            StationStatusCommand.RaiseCanExecuteChanged();

        private void TrunkRaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e) =>
            TrunkStatusCommand.RaiseCanExecuteChanged();

        private void SlotRaiseCanExcuteCHanged(object sender, DataErrorsChangedEventArgs e) =>
            SlotInfoCommand.RaiseCanExecuteChanged();
    }
}
