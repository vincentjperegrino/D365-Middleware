using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices;

namespace KTI.Moo.Extensions.Domain.NCCI.OctoPOS;

public class InvoiceSearch : ISearch<Extensions.OctoPOS.Model.DTO.Invoices.Search, Extensions.OctoPOS.Model.Invoice>
{

    private readonly ISearch<Extensions.OctoPOS.Model.DTO.Invoices.Search, Extensions.OctoPOS.Model.Invoice> _searchDomain;

    public InvoiceSearch(ISearch<Search, Extensions.OctoPOS.Model.Invoice> searchDomain)
    {
        _searchDomain = searchDomain;
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

    public List<Extensions.OctoPOS.Model.Invoice> GetAll(List<Extensions.OctoPOS.Model.Invoice> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        var SearchResult = _searchDomain.GetAll(initialList, dateFrom, dateTo, pagesize, currentpage);

        if (SearchResult is null || SearchResult.Count <= 0)
        {
            return new List<Extensions.OctoPOS.Model.Invoice>();
        }

        //Update Orders
        SearchResult = Mapped_Allowed_PaymentMethod_and_Tax(SearchResult);

        return SearchResult;
    }

    public List<Extensions.OctoPOS.Model.Invoice> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
    {
        var SearchResult = _searchDomain.GetAll(dateFrom, dateTo, pagesize);

        if (SearchResult is null || SearchResult.Count <= 0)
        {
            return new List<Extensions.OctoPOS.Model.Invoice>();
        }

        //Update Orders
        SearchResult = Mapped_Allowed_PaymentMethod_and_Tax(SearchResult);

        return SearchResult;
    }

    private List<Extensions.OctoPOS.Model.Invoice> Mapped_Allowed_PaymentMethod_and_Tax(List<Extensions.OctoPOS.Model.Invoice> invoices)
    {
        return invoices.Select(invoice => TaxCalculation.UpdateTax(invoice)).ToList();
    }

}
