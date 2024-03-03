using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Fulfillments : ShopifySharp.Fulfillment { 


        public Fulfillments() { }   

        public Fulfillments(Model.Fulfillments fulfillments) {


            if (fulfillments.created_at != default)
            {
                CreatedAt = fulfillments.created_at;
            }

            Id = fulfillments.id;

            //LineItems = fulfillments.?.Select(address => new Address(address));

            OrderId = fulfillments.order_id;
            //Receipt = fulfillments.receipt
            Status = fulfillments.status;
            //LocationId = fulfillments.l
            //Email = fulfillments
            TrackingCompany = fulfillments.tracking_company;
            TrackingNumber = fulfillments.tracking_number;


            if (fulfillments.updated_at != default)
            {
                UpdatedAt = fulfillments.updated_at;
            }


        }

    }
    


}
