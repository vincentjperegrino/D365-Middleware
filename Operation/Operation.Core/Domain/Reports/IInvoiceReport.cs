namespace KTI.Moo.Operation.Core.Domain.Reports;

public interface IInvoiceReport<GenericExtensionInvoiceModel, GenericBaseInvoiceModel> : IReport<GenericExtensionInvoiceModel, GenericBaseInvoiceModel> where GenericExtensionInvoiceModel : KTI.Moo.Extensions.Core.Model.InvoiceBase where GenericBaseInvoiceModel : KTI.Moo.Base.Model.InvoiceBase
{
}
