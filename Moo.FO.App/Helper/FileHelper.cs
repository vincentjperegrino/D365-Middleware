using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.App.Helper
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
