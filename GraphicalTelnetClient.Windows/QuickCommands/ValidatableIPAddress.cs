using System.ComponentModel.DataAnnotations;

namespace GraphicalTelnetClient.Windows.QuickCommands
{
    public class ValidatableIPAddress : ValidatableBindableBase
    {
        private string _ipAddress;
        [Display(Name = "IP Address")]
        [Required]
        [RegularExpression(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$", ErrorMessage = "{0} must be a valid IP address.")]
        public string IPAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }

    }
}
