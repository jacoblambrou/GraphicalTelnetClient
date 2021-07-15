using System;
using System.ComponentModel.DataAnnotations;

namespace GraphicalTelnetClient.Windows.QuickCommands
{
    public class ValidatablePortStatus : ValidatableBindableBase
    {
        private string _startPort;
        [Display(Name = "Start Port")]
        [Required]
        [Range(1, 65535, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        public string StartPort
        {
            get { return _startPort; }
            set { SetProperty(ref _startPort, value); }
        }

        private string _endPort;
        [Display(Name = "End Port")]
        [Required]
        [Range(1, 65535, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        public string EndPort
        {
            get { return _endPort; }
            set { SetProperty(ref _endPort, value); }
        }

        public string StartHexPort =>
            ConvertToFourDigitHexString(StartPort);

        public string EndHexPort =>
            ConvertToFourDigitHexString(EndPort);

        private string ConvertToFourDigitHexString(string portNumber)
        {
            portNumber = ConvertToHex(portNumber);
            return EnsureFourDigitHexString(portNumber);
        }

        private string ConvertToHex(string portNumber) =>
            int.Parse(portNumber).ToString("X");

        private string EnsureFourDigitHexString(string portNumber)
        {
            while (portNumber.Length < 4)
                portNumber = $"0{portNumber}";
            return portNumber;
        }
    }
}
