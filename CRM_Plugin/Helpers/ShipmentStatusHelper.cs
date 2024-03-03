using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Helpers
{
    public class ShipmentStatus
    {
        public static readonly int New = 959_080_000;
        public static readonly int Release = 959_080_001;
        public static readonly int InTransit = 959_080_002;
        public static readonly int Complete = 959_080_003;
        public static readonly int Dispatch = 959_080_004;
    }
}
