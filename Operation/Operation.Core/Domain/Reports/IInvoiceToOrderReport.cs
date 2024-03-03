namespace KTI.Moo.Operation.Core.Domain.Reports;

public interface IInvoiceToOrderReport<GenericExtensionInvoiceModel, GenericBaseOrderModel> : IReport<GenericExtensionInvoiceModel, GenericBaseOrderModel> where GenericExtensionInvoiceModel : KTI.Moo.Extensions.Core.Model.InvoiceBase where GenericBaseOrderModel : KTI.Moo.Base.Model.OrderBase
{
}
