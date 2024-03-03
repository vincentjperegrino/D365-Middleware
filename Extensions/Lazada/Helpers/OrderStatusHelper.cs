using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Helpers;

public class OrderStatusHelper
{
    public static string unpaid = "unpaid";
    public static string pending = "pending";
    public static string packed = "packed";
    public static string repacked = "repacked";
    public static string ready_to_ship_pending = "ready_to_ship_pending";
    public static string ready_to_ship = "ready_to_ship";
    public static string shipped = "shipped";
    public static string shipped_back = "shipped_back";
    public static string shipped_back_success = "shipped_back_success";
    public static string shipped_back_failed = "shipped_back_failed";
    public static string failed_delivery = "failed_delivery";
    public static string lost_by_3pl = "lost_by_3pl";
    public static string destroyed_by_3pl = "destroyed_by_3pl";
    public static string delivered = "delivered";
    public static string returned = "returned";
    public static string canceled = "canceled";
    public static string cancelled = "cancelled";
}
