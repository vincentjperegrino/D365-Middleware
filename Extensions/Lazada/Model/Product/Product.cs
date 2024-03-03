using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class Product : Core.Model.ProductBase
    {
        [Required]
        public int primary_category { get; set; }
        public string short_description { get; set; } = "";

        /// <summary>
        /// A list of product SKUs. Required. Has to have at least one item.
        /// </summary>
        public List<Sku> skus { get; set; }
        /// <summary>
        /// Category-specific product attributes. "Key props," according to Lazada. Optional.
        /// </summary>
        public Dictionary<string, string> attributes { get; set; }

        [Required]
        public string brand { get; set; }

        // optional common attributes
        public string model { get; set; } = null;
        public string warranty { get; set; } = null;
        public string warranty_type { get; set; } = null;
        public string product_warranty { get; set; } = null;

        // lazada-generated fields
        public long item_id { get; set; }
        public DateTime? created_time { get; set; } = null;
        // public long created_time
        // {
        //     set
        //     {
        //         CreatedOn = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(value);
        //     }
        // }
        public DateTime? updated_time { get; set; } = null;
        // public long updated_time
        // {
        //     set
        //     {
        //         UpdatedOn = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(value);
        //     }
        // }
        /// <summary>
        /// Product status. Returned by Lazada. Returned value is one of "Active," "Inactive," "Pending QC," "Suspended," or "Deleted."
        /// </summary>
        public string status { get; set; } = null;
        public List<Images> images { get; set; }

        /// <summary>
        /// Product rejection status. Returned by Lazada. Returned value is one of "Lock," "Reject," "Live_Reject," or "Admin."
        /// </summary>
        public string reject_status { get; set; } = null;
        /// <summary>
        /// A list of rejected SKUs and their reason for rejection.
        /// </summary>
        public List<SuspendedSku> suspended_skus { get; set; } = null;

        public Product()
        {
            this.images = new();
            this.attributes = new();
            this.skus = new();
        }

        public Product(int categoryId)
        {
            this.primary_category = categoryId;
            this.images = new();
            this.attributes = new();
            this.skus = new();
        }

        /// <summary>
        /// Constructs a Product from json data.
        /// </summary>
        /// <param name="productJson">The json response data returned by Lazada.</param>
        public Product(string json)
        {
            // JsonSerializerOptions so = new() { PropertyNameCaseInsensitive = false };
            JsonElement productJson = JsonDocument.Parse(json).RootElement;

            this.primary_category = productJson.GetProperty("primary_category").GetInt32();
            this.item_id = productJson.GetProperty("item_id").GetInt64();
            this.created_time = DateTime.UnixEpoch.AddMilliseconds(double.Parse(productJson.GetProperty("created_time").ToString()));
            this.updated_time = DateTime.UnixEpoch.AddMilliseconds(double.Parse(productJson.GetProperty("updated_time").ToString()));
            this.status = productJson.GetProperty("status").ToString();

            List<string> ListStringOFImage = productJson.GetProperty("images").EnumerateArray().Select(a => a.ToString()).ToList();
            this.images = ListStringOFImage.Select(img => new Images() { primary = false, url = img }).ToList();
            if (images.Count > 0)
            {
                this.images.FirstOrDefault().primary = true;
            }

            JsonElement attributes = productJson.GetProperty("attributes");
            this.name = attributes.GetProperty("name").ToString();
            this.description = attributes.GetProperty("description").ToString();
            this.brand = attributes.GetProperty("brand").ToString();

            JsonElement e;
            if (attributes.TryGetProperty("short_description", out e))
                this.short_description = e.ToString();
            if (attributes.TryGetProperty("model", out e))
                this.model = e.ToString();
            if (attributes.TryGetProperty("warranty", out e))
                this.warranty = e.ToString();
            if (attributes.TryGetProperty("warranty_type", out e))
                this.warranty_type = e.ToString();
            if (attributes.TryGetProperty("product_warranty", out e))
                this.product_warranty = e.ToString();

            this.attributes = new();
            foreach (JsonProperty p in attributes.EnumerateObject())
            {
                if (!(new string[] { "name", "description", "short_description", "brand", "model", "warranty", "warranty_type", "product_warranty" }.Contains(p.Name)))
                    this.attributes.Add(p.Name, p.Value.ToString());
            }

            this.skus = productJson.GetProperty("skus").EnumerateArray().Select(s => new Model.Sku(s.ToString())).ToList();
        }


        /// <summary>
        /// Serializes the product instance to json.
        /// </summary>
        /// <returns>A json representation of the product and its skus.</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Serializes the product to xml for Lazada's CreateProduct api method.
        /// </summary>
        /// <returns>An xml representation of the product to be created.</returns>
        public XElement ToAddXML()
        {
            // add required attributes
            XElement attributes = new("Attributes",
                new XElement("name", this.name),
                new XElement("description", this.description),
                new XElement("short_description", this.short_description),
                new XElement("brand", this.brand)
            );

            // add optional attributes if they have values
            if (!string.IsNullOrWhiteSpace(this.model))
                attributes.Add(new XElement("model", this.model));
            if (!string.IsNullOrWhiteSpace(this.warranty))
                attributes.Add(new XElement("warranty", this.warranty));
            if (!string.IsNullOrWhiteSpace(this.warranty_type))
                attributes.Add(new XElement("warranty_type", this.warranty_type));
            if (!string.IsNullOrWhiteSpace(this.product_warranty))
                attributes.Add(new XElement("product_warranty", this.product_warranty));

            // add optional key props if existing
            foreach (KeyValuePair<string, string> a in this.attributes)
            {
                attributes.Add(new XElement(a.Key, a.Value));
            }

            // add skus
            XElement skus = new("Skus");
            foreach (Sku s in this.skus)
                skus.Add(s.ToAddXML());

            XElement product = new XElement("Product",
                new XElement("PrimaryCategory", this.primary_category),
                // variations,
                attributes,
                skus
            );

            if (this.images is not null && this.images.Count > 0)
            {
                product.Add(new XElement("Images", this.images.Select(image => new XElement("Image", image.url))));
            }

            return product;
        }

        /// <summary>
        /// Serializes the product to xml for Lazada's UpdateProduct api method.
        /// </summary>
        /// <returns>An sml representation of the product to be updated.</returns>
        public XElement ToUpdateXML()
        {
            XElement attributes = new("Attributes",
                    new XElement("name", this.name),
                    new XElement("description", this.description),
                    new XElement("short_description", this.short_description)
                   // new XElement("brand", this.brand)
                );

            XElement skus = new("Skus");
            foreach (Sku s in this.skus)
            {
                skus.Add(s.ToUpdateXML());
            }


            if (this.images.Count > 0)
            {
                XElement product = new XElement("Product",
                    new XElement("ItemId", this.item_id),
                    attributes,
                    skus,
                    new XElement("Images", this.images.Select(image => new XElement("Image", image.url)))
                );
                return product;
            }

            XElement NoImageproduct = new XElement("Product",
              new XElement("ItemId", this.item_id),
              attributes,
              skus
              );

            return NoImageproduct;
        }

        public XElement ToUpdatePriceQuantityXML()
        {
            XElement skus = new XElement("Skus");
            foreach (Sku s in this.skus)
            {
                skus.Add(s.ToUpdatePriceQuantityXML());
            }

            XElement product = new XElement("Product",
                skus
            );

            return product;
        }
        public XElement ToUpdatePriceXML()
        {
            XElement skus = new XElement("Skus");
            foreach (Sku s in this.skus)
            {
                skus.Add(s.ToUpdatePriceXML());
            }

            XElement product = new XElement("Product",
                skus
            );

            return product;
        }



        public XElement ToUpdateSellableQuantityXML()
        {
            XElement skus = new XElement("Skus");
            foreach (Sku s in this.skus)
            {
                skus.Add(s.ToUpdateSellableQuantityXML());
            }

            XElement product = new XElement("Product",
                skus
            );

            return product;
        }


        public XElement ToUpdatePriceSalesDateRangeXML(decimal SalesPrice, DateTime SaleStartDate, DateTime SalesEndDate)
        {
            XElement skus = new XElement("Skus");
            foreach (Sku s in this.skus)
            {
                skus.Add(s.ToUpdatePriceSalesDateRangeXML(SalesPrice, SaleStartDate, SalesEndDate));
            }

            XElement product = new XElement("Product",
                skus
            );

            return product;
        }
    }


    /// <summary>
    /// Member class of Product.
    /// </summary>
    public class Sku
    {
        // attributes
        public Dictionary<string, string> Attributes { get; set; }

        public int quantity { get; set; }
        public string package_content { get; set; }
        public List<Images> Images { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string SellerSku { get; set; }
        [Required]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal package_height { get; set; }
        [Required]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal package_length { get; set; }
        [Required]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal package_width { get; set; }
        [Required]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal package_weight { get; set; }
        [Required]
        public decimal price { get; set; }

        // lazada-generated fields
        public string Status { get; set; } = null;
        public int? Available { get; set; } = null;
        public string ShopSku { get; set; } = null;
        public long? SkuId { get; set; } = null;
        public string Url { get; set; } = null;

        public Sku()
        {
            this.Images = new();
            this.Attributes = new();
        }

        public Sku(string json)
        {
            JsonElement skuJson = JsonDocument.Parse(json).RootElement;
            JsonElement e;

            this.quantity = skuJson.GetProperty("quantity").GetInt32();
            if (skuJson.TryGetProperty("package_content", out e))
                this.package_content = e.ToString();
            List<string> ListStringImages = skuJson.GetProperty("Images").EnumerateArray().Select(i => i.ToString()).ToList();

            this.Images = ListStringImages.Select(img => new Images() { primary = false, url = img }).ToList();
            if (Images.Count > 0)
            {
                this.Images.FirstOrDefault().primary = true;
            }


            this.SellerSku = skuJson.GetProperty("SellerSku").ToString();
            this.package_height = decimal.Parse(skuJson.GetProperty("package_height").ToString());
            this.package_length = decimal.Parse(skuJson.GetProperty("package_length").ToString());
            this.package_width = decimal.Parse(skuJson.GetProperty("package_width").ToString());
            this.package_weight = decimal.Parse(skuJson.GetProperty("package_weight").ToString());
            this.price = skuJson.GetProperty("price").GetDecimal();

            this.Status = skuJson.GetProperty("Status").ToString();

            if (skuJson.TryGetProperty("Available", out var AvailableObject))
            {
                this.Available = AvailableObject.GetInt32();
            }

            this.ShopSku = skuJson.GetProperty("ShopSku").ToString();
            this.SkuId = skuJson.GetProperty("SkuId").GetInt64();
            this.Url = skuJson.GetProperty("Url").ToString();
        }

        public XElement ToAddXML()
        {
            XElement sku = new("Sku",
                new XElement("SellerSku", this.SellerSku),
                new XElement("quantity", this.quantity),
                new XElement("price", this.price),
                new XElement("package_height", this.package_height),
                new XElement("package_length", this.package_length),
                new XElement("package_width", this.package_width),
                new XElement("package_weight", this.package_weight),
                new XElement("Images", this.Images.Select(i => new XElement("Image", i.url)))
            );

            foreach (KeyValuePair<string, string> attribute in this.Attributes)
                sku.Add(new XElement(attribute.Key, attribute.Value));

            return sku;
        }

        public XElement ToUpdateXML()
        {
            XElement sku = new("Sku",
                new XElement("SkuId", this.SkuId),
                new XElement("SellerSku", this.SellerSku),
                new XElement("quantity", this.quantity),
                new XElement("package_height", this.package_height),
                new XElement("package_length", this.package_length),
                new XElement("package_width", this.package_width),
                new XElement("package_weight", this.package_weight),
                new XElement("Images", this.Images.Select(i => new XElement("Image", i.url)))
            );

            if (string.IsNullOrWhiteSpace(this.package_content))
            {
                sku.Add(new XElement("package_content", this.package_content));
            }

            return sku;
        }


        public XElement ToUpdatePriceXML()
        {
            XElement sku = new("Sku",
                new XElement("SellerSku", this.SellerSku),
                new XElement("Price", this.price)

            );

            return sku;
        }

        public XElement ToUpdatePriceQuantityXML()
        {
            XElement sku = new("Sku",
                //new XElement("ItemId", itemId),
                //new XElement("SkuId", this.SkuId),
                new XElement("SellerSku", this.SellerSku),
                new XElement("Price", this.price),
                new XElement("Quantity", this.quantity)
            );

            return sku;
        }

        public XElement ToUpdateSellableQuantityXML()
        {
            XElement sku = new("Sku",

                new XElement("SellerSku", this.SellerSku),
                new XElement("SellableQuantity", this.quantity)
            );

            return sku;
        }


        public XElement ToUpdatePriceSalesDateRangeXML(decimal SalesPrice, DateTime SaleStartDate, DateTime SalesEndDate)
        {
            XElement sku = new("Sku",
                //new XElement("ItemId", itemId),
                //new XElement("SkuId", this.SkuId),
                new XElement("SellerSku", this.SellerSku),
                new XElement("Price", this.price),
                new XElement("SalePrice", this.price),
                new XElement("SaleStartDate", SaleStartDate.ToString("yyyy-MM-dd")),
                new XElement("SaleEndDate", SalesEndDate.ToString("yyyy-MM-dd"))
            );

            return sku;
        }

    }

    // public class VariantProperties
    // {
    //     public string Name { get; set; }
    //     public bool HasImage { get; set; } = false;
    //     public bool Customize { get; set; } = true;
    //     public List<string> Options { get; set; }
    // }

    // response object; values will come from lazada
    public record SuspendedSku
    {
        /// <summary>
        /// The reason why the product sku was rejected.
        /// </summary>
        public string RejectReason { get; set; }
        /// <summary>
        /// The seller-defined ID of the rejected sku.
        /// </summary>
        public string SellerSku { get; set; }
        /// <summary>
        /// The Lazada-defined ID of the rejected sku.
        /// </summary>
        public long SkuId { get; set; }
    }
}