using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KTI.Moo.Cyware.Helpers
{
    public class PollMapping
    { 
        public string FormatStringAddSpacePadding(string input, int maxLength)
        {
            // Remove special characters using regular expressions
            string cleanedString = Regex.Replace(input ?? "", "[^a-zA-Z0-9 -]", "");
           
            // Pad the string with empty spaces if necessary
            if (cleanedString.Length < maxLength)
            {
                cleanedString = cleanedString.PadRight(maxLength);
            }
            else if (cleanedString.Length > maxLength)
            {
                //cleanedString = cleanedString.Substring(0, maxLength);
                ////throw exception instead
                ///
                throw new Exception($"{input} exceeds the maximum character limit for a string.");
            }

            return cleanedString;
        }

        public string FormatDateToyyyyMMdd(DateTime? date)
        {
            if (date.HasValue)
            {
                string formattedDate = date.Value.ToString("yyyyMMdd");
                return formattedDate;
            }
            else
            {
                return "        ";
            }
        }

        public string ConcatenateValues(object obj)
        {

            StringBuilder stringBuilder = new StringBuilder();
            var properties = obj.GetType().GetProperties().OrderBy(prop => prop.GetCustomAttribute<SortOrderAttribute>()?.Order ?? int.MaxValue);

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                stringBuilder.Append(value);

            }

            return stringBuilder.ToString();
        }

        public string ConcatenateValuesWithIdentifier(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var properties = obj.GetType().GetProperties().OrderBy(prop => prop.GetCustomAttribute<SortOrderAttribute>()?.Order ?? int.MaxValue);

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                stringBuilder.Append(value + "/");
            }

            return stringBuilder.ToString();
        }

        public string FormatIntAddZeroPrefix(string number, int maxLength)
        {
            //Convert number to string
            string formattedInt = number;

            if (formattedInt.Length < maxLength)
            {
                int numberOfZeros = maxLength - formattedInt.Length;
                //Add '0' depends on the difference of maxLen and stringLen
                string zeroPrefix = new string('0', numberOfZeros);
                formattedInt = zeroPrefix + formattedInt;
            }
            else if (formattedInt.Length > maxLength)
            {
                throw new Exception($"{number} exceeds the maximum character limit for integers.");
            }
            else
            {
                //do nothing
            }
            return formattedInt;
        }

        public string FormatDecimalAddZeroPrefixAndSuffix(string number, int maxLength, int decimalPlaces)
        {
            // Find the index of the decimal point
            int decimalIndex = number.IndexOf('.');

            // If there's no decimal point, add it to the end of the number
            if (decimalIndex == -1)
            {
                number += ".";
                decimalIndex = number.Length - 1;
            }


            // if given_number decimal places is > decimal length..  throw error
            //if(number.substring(decimalindex + 1).length > decimalplaces)
            //{
            //    throw new exception("decimalvalue is greater than the decimal places");
            //}

            // Split the number into integer part and decimal part
            string integerPart = number.Substring(0, decimalIndex).Replace(",","");
            string decimalPart = number.Substring(decimalIndex + 1);

            // Add zero suffixes to the decimal part if necessary
            while (decimalPart.Length < decimalPlaces)
            {
                decimalPart += "0";
            }

            // Combine the integer part and the formatted decimal part
            string formattedNumber = integerPart + decimalPart;

            // Add zero prefixes if necessary
            while (formattedNumber.Length < maxLength)
            {
                formattedNumber = "0" + formattedNumber;
            }

            return formattedNumber;
        }
    }
}
