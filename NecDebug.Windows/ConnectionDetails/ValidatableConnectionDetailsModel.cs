using System.ComponentModel.DataAnnotations;

namespace NecDebug.Windows.ConnectionDetails
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
        //[Range(1, 65535, ErrorMessage = "Server port must be between 1 and 65535")]
        [CustomValidation(typeof(ValidationHelpers), "ServerPortOutOfRange")]
        public int ServerPort
        {
            get { return _serverPort; }
            set { SetProperty(ref _serverPort, value); }
        }
    }
}
