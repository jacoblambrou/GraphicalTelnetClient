using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows.QuickCommands
{
    public class ValidatableSlotNumber : ValidatableBindableBase
    {
        private string _slotNumber;
        [Display(Name = "Slot Number")]
        [Required]
        [Range(1, 24, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        public string SlotNumber
        {
            get { return _slotNumber; }
            set { SetProperty(ref _slotNumber, value); }
        }
    }
}
