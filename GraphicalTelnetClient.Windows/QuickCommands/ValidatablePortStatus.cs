using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows.QuickCommands
{
    public class ValidatablePortStatus : ValidatableBindableBase
    {
        private string _startPort;
        [Display(Name = "start port")]
        [Required]
        [Range(1,65535, ErrorMessage = "Start port must be a number between 1 and 65535.")]
        public string StartPort
        {
            get { return _startPort; }
            set { SetProperty(ref _startPort, value); }
        }

        private string _endPort;
        [Display(Name = "end port")]
        [Required]
        [Range(1, 65535, ErrorMessage = "End port must be a number between 1 and 65535.")]
        public string EndPort
        {
            get { return _endPort; }
            set { SetProperty(ref _endPort, value); }
        }
    }
}
