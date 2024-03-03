
namespace CRM_Plugin.Core.Helper.EntityHelper
{
    public class SalesOrder : BaseEntity
    {
        new public static string entity_name = "salesorder";
        new public static string entity_id = "salesorderid";

        public static string orderStatus = "kti_orderstatus";
        public static string orderpreparedpickedremarks = "kti_orderpreparedpickedremarks";
        public static string confirmOrder = "kti_confirmorder";
        public static string dtmForPayment = "kti_dtmorderforpayment";
        public static string estimatedWeight = "kti_estimatedweight";
        public static string generatePaymentLink = "kti_generatepaymentlink";
        public static string dtmOrderPrepared = "kti_dtmorderprepared";
        public static string orderPrepared = "kti_orderprepared";
        public static string orderPacked = "kti_orderpacked";
        public static string dtmOrderPacked = "kti_dtmorderpacked";
        public static string deliveryMethod = "kti_deliverymethod";
        public static string orderReleased = "kti_orderreleased";
        public static string dtmOrderReleased = "kti_dtmorderreleased";
        public static string orderReleasedRemarks = "kti_orderreleasedremarks";
        public static string dimHeight = "kti_dimheight";
        public static string dimLenght = "kti_dimlength";
        public static string dimWeight = "kti_dimweight";
        public static string dimWidth = "kti_dimwidth";
        public static string shipToLine1 = "shipto_line1";
        public static string shipToLine2 = "shipto_line2";
        public static string shipToLine3 = "shipto_line3";
        public static string description = "description";
        public static string freightAmount = "freightamount";
        public static string shipToCity = "shipto_city";
        public static string shipToContactName = "shipto_contactname";
        public static string shipToCountry = "shipto_country";
        public static string shipToFax = "shipto_fax";
        public static string shipToTelephone = "shipto_telephone";
        public static string shipToName = "shipto_name";
        public static string shipToStateProvince = "shipto_stateorprovince";
        public static string shipToPostalCode = "shipto_postalcode";
        public static string customer = "customerid";
        public static string totalamount = "totalamount";
        public static string paymentterms = "kti_paymenttermscode";
        public static string ordernumber = "ordernumber";
        public static string branchAssigned = "kti_branchassigned";
        public static string receiveddelivery = "kti_receiveddelivery";
        public static string receiveddelivery_datetime = "kti_dtmreceiveddelivery";
        public static string substatus = "kti_substatus";
        public static string orderpackedremarks = "kti_orderpackedremarks";
        public static string deliveredtocustomer = "kti_deliveredtocustomer";
        public static string ispricelocked = "ispricelocked";

        public static string statecode = "statecode";
        public static string modifiedon = "modifiedon";
        public static string name = "name";

        public static string kti_sourceid = "kti_sourceid";
        public static string kti_socialchannelorigin = "kti_socialchannelorigin";

    }
}
