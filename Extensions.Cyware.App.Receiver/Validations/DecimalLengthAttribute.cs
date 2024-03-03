using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Cyware.App.Receiver.Receivers.Validations
{
    public class DecimalLengthAttribute : ValidationAttribute
    {
        private readonly int _length;
        private readonly int _decimalPlaces;

        public DecimalLengthAttribute(int length, int decimalPlaces)
        {
            _length = length;
            _decimalPlaces = decimalPlaces;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal decimalValue)
            {
                string stringValue = decimalValue.ToString();
                int indexOfDecimal = stringValue.IndexOf('.');
                int length = indexOfDecimal >= 0 ? stringValue.Length - 1 : stringValue.Length;

                if (length > _length || (indexOfDecimal >= 0 && stringValue.Substring(indexOfDecimal + 1).Length > _decimalPlaces))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
