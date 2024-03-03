using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.App.Helper
{
    public class StringHelper
    {
        public static int CountKeywordOccurrences(string input, string keyword)
        {
            // Case-insensitive count of keyword occurrences
            int count = 0;
            int index = input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);

            while (index != -1)
            {
                count++;
                index = input.IndexOf(keyword, index + 1, StringComparison.OrdinalIgnoreCase);
            }

            return count;
        }
    }
}
