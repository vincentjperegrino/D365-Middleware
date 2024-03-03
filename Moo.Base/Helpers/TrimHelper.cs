namespace KTI.Moo.Base.Helpers;

public static class Trim
{
    public static string TrimFirst100Characters(this string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return string.Empty;
        }

        if (inputString.Length <= 100)
        {
            return inputString;
        }

        return inputString[..100];
    }
}
