using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Helper
{
    public class TimeConversion
    {
        static bool IsValidMilitaryTime(string input)
        {
            //Check if the input is exactly 4 digits and each digit is a number
            if (input.Length == 4 && int.TryParse(input, out _))
            {
                int hours = int.Parse(input.Substring(0, 2));
                int minutes = int.Parse(input.Substring(2, 2));

                // Check if hours and minutes are within valid military time ranges
                if (hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59)
                {
                    return true;
                }
            }

            return false;
        }


        public static int ConvertMilitaryTimeToSeconds(string militaryTime)
        {
            try
            {
                ///Add 0 padding if input < 4 digits. 
                militaryTime = militaryTime.Length < 5 ? militaryTime.Trim() : throw new Exception($"Invalid military time format({militaryTime}). Please use HHmm (e.g., 1430 for 2:30 PM).");
                int numberOfZeros = 4 - militaryTime.Length;

                string zeroPrefix = new string('0', numberOfZeros);
                string formattedTime = zeroPrefix + militaryTime;

                if (IsValidMilitaryTime(formattedTime))
                {
                    int hours = int.Parse(formattedTime.Substring(0, 2));
                    int minutes = int.Parse(formattedTime.Substring(2, 2));

                    int totalSeconds = (hours * 3600) + (minutes * 60);

                    return totalSeconds;
                }
                else
                {
                    throw new Exception($"Invalid military time format({militaryTime}). Please use HHmm (e.g., 1430 for 2:30 PM).");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
