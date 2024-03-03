using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Helper;

public class InventoryPurposeHelper
{

    public static int InTransit = 1;
    public static int ForSales = 2;
    public static int NotForSales_Reserved = 3;
    public static int NotForSales_ForCollection = 4;
    public static int NotForSales_DisplayItem = 5;
    public static int NotForSales_ForRepair = 6;
    public static int NotForSales_Others = 7;
    public static int Expire_Item = 8;
    public static int Damage_Item = 9;
    public static int Discontinued = 10;
    public static int Return_Merchandise_Authorisation = 11;
}
