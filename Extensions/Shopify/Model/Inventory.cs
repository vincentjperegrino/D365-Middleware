using KTI.Moo.Extensions.Shopify.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;


public class Inventory : KTI.Moo.Extensions.Core.Model.InventoryBase
{
    public Inventory()
    {


    }

    public Inventory(ShopifySharp.InventoryItem inventory)

    {

        cost = inventory.Cost ?? default;
        country_code_of_origin = inventory.CountryCodeOfOrigin;
        created_at = inventory.CreatedAt?.DateTime ?? default;
        harmonized_system_code = inventory.HarmonizedSystemCode;
        id = inventory.Id ?? default;
        province_code_of_origin = inventory.ProvinceCodeOfOrigin;
        sku = inventory.SKU;
        tracked = inventory.Tracked ?? default;
        updated_at = inventory.UpdatedAt?.DateTime ?? default;
        requires_shipping = inventory.RequiresShipping ?? default;

        country_harmonized_system_codes = inventory.CountryHarmonizedSystemCodes?.Select(countryHarmonizedSystemCode => new CountryHarmonizedSystemCode(countryHarmonizedSystemCode)).ToList();


    }

    public decimal cost { get; set; }
    public string country_code_of_origin { get; set; }
    public List<CountryHarmonizedSystemCode> country_harmonized_system_codes { get; set; }
    public DateTime created_at { get; set; }
    public override DateTime createdon { get => created_at; set => created_at = value; }
    public string harmonized_system_code { get; set; }
    public long id { get; set; }
    public string province_code_of_origin { get; set; }
    public string sku { get; set; }
    public bool tracked { get; set; }
    public DateTime updated_at { get; set; }
    public bool requires_shipping { get; set; }





}

