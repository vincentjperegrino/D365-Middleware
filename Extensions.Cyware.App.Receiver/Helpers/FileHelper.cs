using System.IO;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Helpers
{
    public class FileHelper
    {
        public static string GetFileName(string input)
        {
            string fileName = Path.GetFileName(input);
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
