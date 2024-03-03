using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain;


/// <summary>
/// Mostly For Platform Channels (Lazada and Shoppe)
/// </summary>
public interface IFulfillment<T, K> where T : Core.Model.PackageBase where K : Core.Model.PackageItemBase
{
    bool DeliverDigital(string orderid, List<string> orderitemids);

    /// <summary>
    /// Get Shipping Allocate type for the orders
    /// </summary>
    /// <param name="order"></param>
    /// <param name="orderitems"></param>
    /// <returns></returns>
    string GetShipmentProvider(string orderid, List<string> orderitemids);

    /// <summary>
    /// Get Package ID for items
    /// </summary>
    /// <param name="order"></param>
    /// <param name="orderitems"></param>
    /// <param name="shipping_allocate_type"></param>
    /// <param name="delivery_type"></param>
    /// <param name="shipment_provider_code">Optional see lazada documentation for the use</param>
    /// <returns></returns>
    T Pack(string orderid, List<string> orderitemids, string shipping_allocate_type);

    T Pack(string orderid, List<string> orderitemids, string shipping_allocate_type, string delivery_type);

    T Pack(string orderid, List<string> orderitemids, string shipping_allocate_type, string delivery_type, string shipment_provider_code);

    /// <summary>
    /// Get AWB pdf from lazada
    /// </summary>
    /// <param name="packageid"></param>
    /// <returns></returns>
    string PrintAWB(string packageid);

    /// <summary>
    /// Mark as ready to ship
    /// </summary>
    /// <param name="packageid"></param>
    /// <returns></returns>
    bool ReadyToShip(string packageid);

    /// <summary>
    /// Mark as Repacked
    /// </summary>
    /// <param name="packageid"></param>
    /// <returns></returns>
    K RecreatePackage(string packageid);

}
