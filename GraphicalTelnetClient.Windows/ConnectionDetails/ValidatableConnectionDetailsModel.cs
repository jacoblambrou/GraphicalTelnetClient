using System.ComponentModel.DataAnnotations;

namespace GraphicalTelnetClient.Windows.ConnectionDetails
{
    public class ValidatableConnectionDetailsModel : ValidatableBindableBase
    {
        private string _serverAddress;
        [Display(Name = "server address")]
        [Required]
        public string ServerAddress
        {
            get { return _serverAddress; }
            set { SetProperty(ref _serverAddress, value); }
        }

        private int _serverPort;
        [Display(Name = "Server port")]
        [CustomValidation(typeof(ValidationHelpers), "ServerPortOutOfRange")]
        public int ServerPort
        {
            get { return _serverPort; }
            set { SetProperty(ref _serverPort, value); }
        }
    }
}
