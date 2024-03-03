using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using KTI.Moo.Extensions.OctoPOS.Helper;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class Order : OrderBase
    {
        private DateTime _CreateDateTime;
        private DateTime _SalesOrderDate;


        [JsonProperty("SalesOrderDate")]
        public DateTime SalesOrderDate
        {
            get => _SalesOrderDate;
            set => _SalesOrderDate = Helper.DateTimeHelper.PHT_to_UTC(value);
        }

        [JsonProperty("CreateDateTime")]
        public DateTime CreateDateTime
        {
            get => _CreateDateTime;
            set => _CreateDateTime = Helper.DateTimeHelper.PHT_to_UTC(value);
        }

        [JsonIgnore]
        public override int companyid { get; set; }

        //primary id in OctoPOS
        [JsonProperty("SalesOrderId")]
        public int SalesOrderId { get; set; }

        //external sales order number
        //use for adding salesorder must be unique
        [JsonProperty("SalesOrderNumber")]
        public string SalesOrderNumber { get; set; }

        [JsonIgnore]
        [StringLength(300)]

        public override string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.name))
                {
                    return SalesOrderNumber;
                }

                return base.name;
            }
            set => base.name = value;
        }

        [JsonProperty("DeliveryAddress1")]
        public string DeliveryAddress1
        {
            get => CustomerDetails.Address1;

            set
            {
                CustomerDetails.Address1 = value;
                CustomerDetails.ShippingAddress = value;
            }

        }

        [JsonProperty("DeliveryAddress2")]
        public string DeliveryAddress2
        {
            get => CustomerDetails.Address2;
            set
            {
                CustomerDetails.Address2 = value;
                CustomerDetails.ShippingAddress2 = value;
            }
        }




        [JsonIgnore]
        [StringLength(80)]
        public override string shipto_line1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.shipto_line1))
                {
                    return DeliveryAddress1;
                }

                return base.shipto_line1;
            }

            set => base.shipto_line1 = value;
        }


        [JsonIgnore]
        [StringLength(80)]
        public override string shipto_line2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.shipto_line2))
                {
                    return DeliveryAddress2;
                }

                return base.shipto_line2;
            }

            set => base.shipto_line2 = value;
        }

        [JsonIgnore]
        [StringLength(80)]
        public override string billto_line1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.billto_line1))
                {
                    return DeliveryAddress1;
                }

                return base.billto_line1;
            }

            set => base.billto_line1 = value;
        }

        [JsonIgnore]
        [StringLength(80)]
        public override string billto_line2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.billto_line2))
                {
                    return DeliveryAddress2;
                }

                return base.billto_line2;
            }

            set => base.billto_line2 = value;
        }




        [JsonProperty("CustomerCode")]
        public string CustomerCode
        {
            get => CustomerDetails.CustomNumber;
            set => CustomerDetails.CustomNumber = value;
        }


        [JsonProperty("Remark")]
        public override string description { get; set; }


        [JsonProperty("CashierCode")]
        public string CashierCode { get; set; } = CashierCodeHelper.Admin;

        [JsonProperty("Status")]
        public int Status { get; set; } = 1;

        [JsonProperty("Terminal")]
        public string Terminal { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("DeliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("NetSalesAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal NetSalesAmount { get; set; }

        [JsonProperty("TotalDiscountAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TotalDiscountAmount { get; set; }

        [JsonProperty("TaxAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxAmount { get; set; }

        [JsonProperty("IsTaxExclusive", DefaultValueHandling = DefaultValueHandling.Include)]
        public int IsTaxExclusive { get; set; }

        [JsonProperty("SalesOrderType")]
        public string SalesOrderType { get; set; }

        [JsonProperty("WholeReceiptDiscount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal WholeReceiptDiscount { get; set; }

        [JsonProperty("ShippingCost", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal freightamount { get; set; }

        [JsonProperty("TaxPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxPercentage { get; set; }

        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("PointsEarned", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal PointsEarned { get; set; }

        [JsonProperty("DepositNumber")]
        public string DepositNumber { get; set; }

        [JsonProperty("ReserveInventory", DefaultValueHandling = DefaultValueHandling.Include)]
        public int ReserveInventory { get; set; }

        [JsonProperty("SalesOrderItems")]
        public List<OrderItem> SalesOrderItems { get; set; }

        [JsonProperty("DepositItems")]
        public List<OrderDepositItem> DepositItems { get; set; }

        [JsonIgnore]
        public Customer CustomerDetails { get; set; }


        public Order()
        {

            SalesOrderItems = new();
            DepositItems = new();
            CustomerDetails = new();
            SalesOrderType = SalesOrderTypeHelper.DefaultSalesOrderType;
        }

    }
}
