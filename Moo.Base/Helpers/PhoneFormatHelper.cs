
namespace KTI.Moo.Base.Helpers;

public static class PhoneFormatHelper
{
    public static string FormatPhoneNumber(this string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return string.Empty;
        }

        // Remove any non-digit characters from the input
        var digitsOnly = new string(mobileNumber.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length < 10)
        {
            return digitsOnly;
        }

        // Extract the last 10 digits from the input using slice notation
        var last10DigitsOnly = digitsOnly[^10..];

        if (digitsOnly.Length == 10)
        {
            return $"+63{last10DigitsOnly}";
        }

        // Extract the remaining digits as the country code
        var countryCode = digitsOnly[..^10].Trim('0');

        if (countryCode.Length == 0)
        {
            return $"+63{last10DigitsOnly}";
        }

        return $"+{countryCode}{last10DigitsOnly}";
    }
}