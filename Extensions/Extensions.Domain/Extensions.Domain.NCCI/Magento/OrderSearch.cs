using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Model.DTO.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Domain.NCCI.Magento;

public class OrderSearch : ISearch<Extensions.Magento.Model.DTO.Orders.Search, Extensions.Magento.Model.Order>
{
    private readonly ISearch<Extensions.Magento.Model.DTO.Orders.Search, Extensions.Magento.Model.Order> _searchDomain;

    public OrderSearch(ISearch<Search, Extensions.Magento.Model.Order> searchDomain)
    {
        this._searchDomain = searchDomain;
    }

    public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {
        var SearchResult = _searchDomain.Get(dateFrom, dateTo, pagesize, currentPage);

        if (SearchResult.values is null || SearchResult.values.Count <= 0)
        {
            return new Search();
        }

        //Update Orders
        SearchResult.values = Mapped_Allowed_PaymentMethod_and_Tax(SearchResult.values);

        return SearchResult;
    }

    public List<Extensions.Magento.Model.Order> GetAll(List<Extensions.Magento.Model.Order> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        var SearchResult = _searchDomain.GetAll(initialList, dateFrom, dateTo, pagesize, currentpage);

        if (SearchResult is null || SearchResult.Count <= 0)
        {
            return new List<Extensions.Magento.Model.Order>();
        }

        //Update Orders
        SearchResult = Mapped_Allowed_PaymentMethod_and_Tax(SearchResult);

        return SearchResult;
    }

    public List<Extensions.Magento.Model.Order> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
    {
        var SearchResult = _searchDomain.GetAll(dateFrom, dateTo, pagesize);

        if (SearchResult is null || SearchResult.Count <= 0)
        {
            return new List<Extensions.Magento.Model.Order>();
        }

        //Update Orders
        SearchResult = Mapped_Allowed_PaymentMethod_and_Tax(SearchResult);

        return SearchResult;
    }

    private List<Extensions.Magento.Model.Order> Mapped_Allowed_PaymentMethod_and_Tax(List<Extensions.Magento.Model.Order> orders)
    {
        return orders.Where(orders => orders.order_payment.method == "cashondelivery" || orders.order_payment.method == "phoenix_cashondelivery" || orders.order_status == "processing" || orders.order_status == "complete")
                     .Select(orders => TaxCalculation.UpdateTax(orders)).ToList();
    }


}
