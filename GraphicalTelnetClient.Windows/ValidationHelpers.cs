using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalTelnetClient.Windows
{
    public static class ValidationHelpers
    {
        public static ValidationResult ServerPortOutOfRange(int value, ValidationContext context)
        {
            int minValue = 1;
            int maxValue = 65535;
            try
            {
                //int number = int.Parse(value);
                int number = value;

                if (!(number >= minValue) || !(number <= maxValue))
                    return new ValidationResult($"Server port must be between {minValue} and {maxValue}");

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult($"Server port must be a number between {minValue} and {maxValue}");
            }
        }
    }
}
