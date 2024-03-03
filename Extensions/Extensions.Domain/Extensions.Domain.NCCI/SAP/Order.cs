using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.SAP.Model;
using KTI.Moo.Extensions.SAP.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Domain.NCCI.SAP;

public class Order : IOrder<KTI.Moo.Extensions.SAP.Model.Order, KTI.Moo.Extensions.SAP.Model.OrderItem>
{

    private readonly IOrder<KTI.Moo.Extensions.SAP.Model.Order, KTI.Moo.Extensions.SAP.Model.OrderItem> orderDomain;

    public Order(IOrder<Extensions.SAP.Model.Order, KTI.Moo.Extensions.SAP.Model.OrderItem> orderDomain)
    {
        this.orderDomain = orderDomain;
    }

    public bool IsForDispatch(string FromDispatcherQueue)
    {
        if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
        {
            return false;
        }

        var OrderData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var kti_paymenttermscode = OrderData.ContainsKey("kti_paymenttermscode") ? OrderData["kti_paymenttermscode"].Value<int>() : default;

        var statuscode = OrderData.ContainsKey("statuscode") ? OrderData["statuscode"].Value<int>() : default;

        var channel = OrderData.ContainsKey("U_Channel") ? OrderData["U_Channel"].Value<string>() : default;

        var AllowedChannelReplicate = new string[] { "Magento", "Lazada" };

        if (!AllowedChannelReplicate.Contains(channel))
        {
            return false;
        }

        if (channel == "Lazada")
        {
            return true;
        }


        // if pending 
        if (statuscode == 2)
        {
            //paymentterms //GCASH COD  //COD
            var AllowedinPending = new int[] { 959_080_017, 959_080_002 };

            if (AllowedinPending.Contains(kti_paymenttermscode))
            {
                return true;
            }

            return false;
        }

        //statuscodes //holded  //canceled 
        var notAllowedStatus = new int[] { 959_080_001, 959_080_002 };

        // Not in Not allowed list of status
        if (!notAllowedStatus.Contains(statuscode))
        {
            return true;
        }

        return false;
    }

    public bool IsForReceiver(string FromDispatcherQueue)
    {
        return orderDomain.IsForReceiver(FromDispatcherQueue);
    }




    public Extensions.SAP.Model.Order Upsert(string FromDispatcherQueue)
    {
        var order = orderDomain.Upsert(FromDispatcherQueue);

        var CRMorder = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        if (CRMorder.ContainsKey("kti_orderstatus") && (int)CRMorder["kti_orderstatus"] == KTI.Moo.Base.Helpers.OrderStatus.CancelOrder)
        {
            CancelOrder(order);
        }

        return order;
    }

    public Extensions.SAP.Model.Order Add(Extensions.SAP.Model.Order Order)
    {
        return orderDomain.Add(Order);
    }

    public Extensions.SAP.Model.Order Add(string FromDispatcherQueue)
    {
        return orderDomain.Add(FromDispatcherQueue);
    }

    public Extensions.SAP.Model.Order Get(long id)
    {
        return orderDomain.Get(id);
    }
    public Extensions.SAP.Model.Order Get(string id)
    {
        return orderDomain.Get(id);
    }
    public Extensions.SAP.Model.Order GetByField(string FieldName, string FieldValue)
    {
        return orderDomain.GetByField(FieldName, FieldValue);
    }

    public IEnumerable<OrderItem> GetItems(long id)
    {
        return orderDomain.GetItems(id);
    }


    public Extensions.SAP.Model.Order Update(Extensions.SAP.Model.Order order)
    {
        return orderDomain.Update(order);
    }

    public Extensions.SAP.Model.Order Update(string FromDispatcherQueue, string Orderid)
    {
        return orderDomain.Update(FromDispatcherQueue, Orderid);
    }

    public Extensions.SAP.Model.Order Upsert(Extensions.SAP.Model.Order order)
    {
        return orderDomain.Upsert(order);
    }

    public bool CancelOrder(Extensions.SAP.Model.Order order)
    {
        return orderDomain.CancelOrder(order);
    }

}
