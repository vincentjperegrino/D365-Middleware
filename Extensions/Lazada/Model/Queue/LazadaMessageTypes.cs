using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model.Queue
{
    public enum MessageTypes : short
    {
        // custom message types
        /// <summary>
        /// Authentication code when seller authorizes the app.
        /// </summary>
        AuthCode = 500,
        /// <summary>
        /// Test authentication, contains at least an access token.
        /// </summary>
        TestAuth = 501,
        /// <summary>
        /// Test order update.
        /// </summary>
        TestOrder = 502,
        /// <summary>
        /// Test product update.
        /// </summary>
        TestProduct = 503,

        // lazada message types
        /// <summary>
        /// Triggered when order status changes.
        /// </summary>
        TradeOrder = 0,
        /// <summary>
        /// Triggered when product quality control status changes.
        /// </summary>
        ProductQuality = 1,
        /// <summary>
        /// Triggered when a new product or sku is added.
        /// </summary>
        ProductUpdate = 3,
        /// <summary>
        /// Triggered when warehouse sellable stock number below certain threshold.
        /// </summary>
        ShallowStock = 6,
        /// <summary>
        /// Triggered when a short video status is updated.
        /// </summary>
        ShortVideoState = 7,
        /// <summary>
        /// Triggered if app subscribed to this notification and token has 48 hours before expiry.
        /// </summary>
        AuthorizationToken = 8,
        /// <summary>
        /// Triggered when the category tree has been updated.
        /// </summary>
        ProductCategory = 12,
        /// <summary>
        /// Triggered when seller status changes.
        /// </summary>
        SellerStatus = 13,
        /// <summary>
        /// Triggered when fulfilment order is updated.
        /// </summary>
        FulfilmentOrder = 14,
        /// <summary>
        /// Triggered when reverse order status is updated.
        /// </summary>
        ReverseOrder = 10,
        /// <summary>
        /// Triggered 72 hours before voucher expires.
        /// </summary>
        Promotion = 11
    }
}
