using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class TaxLine
{
    private TaxLine taxLine;

    public TaxLine()
    {


    }

    public TaxLine(ShopifySharp.TaxLine taxLine) {

        title = taxLine.Title;
        price = taxLine.Price ?? default;
        rate = taxLine.Rate ?? default;
        if (taxLine.PriceSet != null)
        { 
            price_set = new Model.PriceSet(taxLine.PriceSet);
           
        }
        //channel_liable = taxLine.channellaible // not available

     
    
    }

    public TaxLine(TaxLine taxLine)
    {
        this.taxLine = taxLine;
    }

    public string title { get; set; }
    public decimal price { get; set; }
    public decimal rate { get; set; }
    public PriceSet price_set { get; set; }
    //public bool channel_liable { get; set; }



}