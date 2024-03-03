

namespace KTI.Moo.Base.Helpers;

public class OrderStatus
{
    public static readonly string InCartName = "InCart";
    public static readonly string ForPaymentName = "ForPayment";
    public static readonly string CheckoutName = "Checkout";
    public static readonly string OrderPreparedName = "OrderPrepared";
    public static readonly string OrderPackedName = "OrderPacked";
    public static readonly string OrderReleasedName = "OrderReleased";
    public static readonly string ReceivedByDeliveryName = "ReceivedByDelivery";
    public static readonly string DriverNearbyName = "DriverNearby";
    public static readonly string DeliveredToCustomerName = "DeliveredToCustomer";
    public static readonly string CancelOrderName = "CancelOrder";
    public static readonly string RequestCancelOrderName = "RequestCancelOrder";
    public static readonly string IncompleteName = "Incomplete";

    public static readonly int InCart = 959_080_000;
    public static readonly int ForPayment = 959_080_009;
    public static readonly int Checkout = 959_080_001;
    public static readonly int OrderPrepared = 959_080_002;
    public static readonly int OrderPacked = 959_080_003;
    public static readonly int OrderReleased = 959_080_004;
    public static readonly int ReceivedByDelivery = 959_080_005;
    public static readonly int DriverNearby = 900_330_001;
    public static readonly int DeliveredToCustomer = 959_080_006;
    public static readonly int CancelOrder = 959_080_007;
    public static readonly int RequestCancelOrder = 959_080_008;
    public static readonly int Incomplete = 959_080_010;

    //Shipment Status Consider as Extension of Order status

    public static readonly string ForDispatchName = "ForDispatch";


    public static int GetOptionSetValue(string name)
    {
        if (name == InCartName)
        {
            return InCart;
        }

        if (name == ForPaymentName)
        {
            return ForPayment;
        }

        if (name == CheckoutName)
        {
            return Checkout;
        }

        if (name == OrderPreparedName)
        {
            return OrderPrepared;
        }

        if (name == OrderPackedName)
        {
            return OrderPacked;
        }

        if (name == OrderReleasedName)
        {
            return OrderReleased;
        }

        if (name == ReceivedByDeliveryName)
        {
            return ReceivedByDelivery;
        }
        
        if (name == DriverNearbyName)
        {
            return DriverNearby;
        }
        
        if (name == DeliveredToCustomerName)
        {
            return DeliveredToCustomer;
        }
        
        if (name == CancelOrderName)
        {
            return CancelOrder;
        }
        
        if (name == RequestCancelOrderName)
        {
            return RequestCancelOrder;
        }
        
        if (name == IncompleteName)
        {
            return Incomplete;
        }

        return 0;
    }




}
