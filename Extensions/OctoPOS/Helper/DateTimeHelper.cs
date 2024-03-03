using System;


namespace KTI.Moo.Extensions.OctoPOS.Helper
{
    public class DateTimeHelper
    {

        public static DateTime PHTnow()
        {
            return DateTime.UtcNow.AddHours(8);
        }

        public static DateTime PHT_to_UTC(DateTime PHTDate)
        {
            return PHTDate.AddHours(-8);
        }

        public static DateTime UTC_to_PHT(DateTime UTCDate)
        {
            return UTCDate.AddHours(8);
        }

    }
}
