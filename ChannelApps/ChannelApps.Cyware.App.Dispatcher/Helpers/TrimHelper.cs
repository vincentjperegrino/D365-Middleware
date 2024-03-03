using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers
{
    public class TrimHelper
    {
        public static string SplitAndTrim(string input, int maxLength, int part)
        {
            if (input == null)
            {
                return ""; // Or return null if you prefer
            }
            else
            {
                // Split the input string into parts based on a maximum length
                string[] parts = input.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                // If there are no parts, return an empty string
                if (parts.Length == 0)
                {
                    return "";
                }
                else
                {
                    // Take the specified part and trim it to the maximum length
                    return parts.Length > part ? parts[part].Trim().Substring(0, Math.Min(parts[part].Trim().Length, maxLength)) : "";
                }
            }
        }
    }
}
