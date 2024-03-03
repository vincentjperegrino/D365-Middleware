using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class StockItem : InventoryBase
    {
        //Quantity of the stocks
        //In magento can do quantity stock adjustment
        [JsonProperty("qty", DefaultValueHandling = DefaultValueHandling.Include)]
        public override double qtyonhand { get; set; }

        //Status of stocks, true = "In stock" , false = "Out of stock" 
        [JsonProperty("is_in_stock", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_in_stock { get; set; }

        //Allow Multiple Boxes for Shipping
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_decimal_divided { get; set; }

        //If Quantity has decimay
        [JsonProperty("is_qty_decimal", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_quantity_decimal { get; set; }

        //primary key
        //1 Product 1 item_id
        [JsonProperty("item_id")]
        public int item_id { get; set; }

        //Date stock becomes low stock
        //Date when quantity reaches below the notify_stock_qty number.
        public DateTime? low_stock_date { get; init; }

        //Products id
        //Foreign key of Stocks from Products
        [JsonProperty("product_id")]
        public int product_id { get; set; }

        //Optional. To show the defailt message in Magento.
        [JsonProperty("show_default_notification_message", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool show_default_notification_message { get; set; }

        //Stock_id
        //Foreign key of Stocks from Inventory
        //Magento uses default stock_id only. stock_id = 1
        [JsonProperty("stock_id")]
        public int stock_id { get; set; }


        //Defines whether products can be automatically returned to stock when the refund for an order is created
        [JsonProperty("stock_status_changed_auto")]
        public int stock_status_changed_auto { get; set; }

        //The customer can place the order for products that are out of stock at the moment 
        //0 - No Backorders
        //1 - Allow Qty Below 0
        //2 - Allow Qty Below 0 and Notify Customer
        [JsonProperty("backorders")]
        public int backorders
        {
            get => BackordersConfig.config_value;
            set => BackordersConfig.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_backorders", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_backorders
        {
            get => BackordersConfig.is_config_enable;
            set => BackordersConfig.is_config_enable = value;
        }

        //Backorders with use_config_backorders
        [JsonIgnore]
        public ConfigurationSettings<int> BackordersConfig { get; set; }


        //If true, quantity can be purchace in an increment depending on the value of quantity_increments
        //If false, quantity_increments is 1
        [JsonProperty("enable_qty_increments", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool enable_quantity_increments
        {
            get => EnableQuantityIncrements.config_value;
            set => EnableQuantityIncrements.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_enable_qty_inc", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_enable_quantity_increments
        {
            get => EnableQuantityIncrements.is_config_enable;
            set => EnableQuantityIncrements.is_config_enable = value;
        }

        //enable_quantity_increments with use_config_enable_qty_inc
        [JsonIgnore]
        public ConfigurationSettings<bool> EnableQuantityIncrements { get; set; }

        //if true, product stocks are manage and track.
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool manage_stock
        {
            get => ManageStock.config_value;
            set => ManageStock.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_manage_stock", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_manage_stock
        {
            get => ManageStock.is_config_enable;
            set => ManageStock.is_config_enable = value;
        }

        //manage_stock with use_config_manage_stock
        [JsonIgnore]
        public ConfigurationSettings<bool> ManageStock { get; set; }

        //Maximum quantity in shopping Cart 
        [JsonProperty("max_sale_qty")]
        public double max_sale_quantity
        {
            get => MaxSaleQuantity.config_value;
            set => MaxSaleQuantity.config_value = value;
        }


        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_max_sale_qty", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_max_sale_quantity
        {
            get => MaxSaleQuantity.is_config_enable;
            set => MaxSaleQuantity.is_config_enable = value;
        }

        //max_sale_qty with use_config_max_sale_qty
        [JsonIgnore]
        public ConfigurationSettings<double> MaxSaleQuantity { get; set; }


        //Quantity of Product to become out of stock
        [JsonProperty("min_qty")]
        public double min_quantity
        {
            get => MinQuantity.config_value;
            set => MinQuantity.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_min_qty", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_min_quantity
        {
            get => MinQuantity.is_config_enable;
            set => MinQuantity.is_config_enable = value;
        }

        //min_qty with use_config_min_qty
        [JsonIgnore]
        public ConfigurationSettings<double> MinQuantity { get; set; }


        // minimum quantity in shopping cart
        [JsonProperty("min_sale_qty")]
        public double min_sale_quantity
        {
            get => MinSaleQuantity.config_value;
            set => MinSaleQuantity.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_min_sale_qty")]
        public int use_config_min_sale_quantity
        {
            get => MinSaleQuantity.is_config_enable ? 1 : 0;
            set => MinSaleQuantity.is_config_enable = value == 1;
        }

        //min_sale_qty and use_config_min_sale_qty
        [JsonIgnore]
        public ConfigurationSettings<double> MinSaleQuantity { get; set; }

        //The number of inventory items below which the customer will be notified via the RSS feed
        [JsonProperty("notify_stock_qty")]
        public double notify_stock_quantity
        {
            get => NotifyStockQuantity.config_value;
            set => NotifyStockQuantity.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_notify_stock_qty", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_notify_stock_quantity
        {
            get => NotifyStockQuantity.is_config_enable;
            set => NotifyStockQuantity.is_config_enable = value;
        }

        //notify_stock_qty with use_config_notify_stock_qty
        [JsonIgnore]
        public ConfigurationSettings<double> NotifyStockQuantity { get; set; }

        // if enable_qty_increments is true, qty_increments is the increments of the product.
        // if qty_increments = 6. Product can be bought in 6,12,18 ....
        [JsonProperty("qty_increments")]
        public double quantity_increments
        {
            get => QuantityIncrements.config_value;
            set => QuantityIncrements.config_value = value;
        }

        //if true, uses global settings of Magento instead
        [JsonProperty("use_config_qty_increments", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool use_config_quantity_increments
        {
            get => QuantityIncrements.is_config_enable;
            set => QuantityIncrements.is_config_enable = value;
        }

        // qty_increments with use_config_qty_increments
        [JsonIgnore]
        public ConfigurationSettings<double> QuantityIncrements { get; set; }


        public StockItem()
        {
            QuantityIncrements = new();
            NotifyStockQuantity = new();
            MinSaleQuantity = new();
            MinQuantity = new();
            MaxSaleQuantity = new();
            ManageStock = new();
            EnableQuantityIncrements = new();
            BackordersConfig = new();

        }

    }



}
