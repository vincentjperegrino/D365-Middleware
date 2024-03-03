using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.CustomDataType
{
    public class Moo
    {

    }

    public class MooDateTime
    {
        public MooDateTime() { }
        public MooDateTime(DateTime _dateTime) 
        {
            this.LocalDateTime = _dateTime;
            this.UTCDateTime = _dateTime.ToUniversalTime();
        }

        public DateTime LocalDateTime { get; set; }
        public DateTime UTCDateTime { get; set; }
    }
}
