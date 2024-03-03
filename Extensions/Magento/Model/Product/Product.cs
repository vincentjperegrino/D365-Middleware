using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Product : ProductBase
    {
        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonIgnore]
        public override string productid { get => id.ToString(); }

        [Required]
        public override string name { get; set; }

        public override string sku { get; set; }

        [JsonIgnore]
        public override string description
        {
            get
            {
                if (custom_attributes != null && custom_attributes.Count > 0 && this.custom_attributes.Any(items => items.attribute_code == "description" && items.value is not null))
                {
                    try
                    {
                        return this.custom_attributes.Where(items => items.attribute_code == "description").Select(item => item.value).FirstOrDefault().ToString();
                    }
                    catch
                    {

                        try
                        {
                            return this.custom_attributes.Where(items => items.attribute_code == "description").Select(item => item.value).FirstOrDefault().ToString();
                        }
                        catch
                        {
                            return default;
                        }
                    }
                }
                return default;
            }

            set
            {

                if (custom_attributes != null && custom_attributes.Count > 0)
                {
                    bool checkifthereisdescription = this.custom_attributes.Any(items => items.attribute_code == "description");
                    if (checkifthereisdescription)
                    {
                        this.custom_attributes.Find(items => items.attribute_code == "description").value = value;

                        return;
                    }

                    this.custom_attributes.Add(new Attribute() { attribute_code = "description", value = value });

                    return;

                }
                else
                {
                    custom_attributes = new();
                    this.custom_attributes.Add(new Attribute() { attribute_code = "description", value = value });
                    return;
                }



            }
        }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int attribute_set_id { get; set; }

        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Include)]
        public override int statuscode { get; set; }

        public int visibility { get; set; }

        public string type_id { get; set; }

        [JsonProperty("weight")]
        public override decimal stockweight { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        public List<Attribute> custom_attributes { get; set; }

        public List<MediaGallery> media_gallery_entries { get; set; }

        public List<ProductOptions> options { get; set; }

        public List<ProductLinks> product_links { get; set; }

        public List<PrdouctTierPrice> tier_prices { get; set; }

        public Product()
        {


            //extension_attributes = new();

            //extension_attributes.stock_item = new();

            //options = new();

            //product_links = new();

            //tier_prices = new();

        }



    }
}
