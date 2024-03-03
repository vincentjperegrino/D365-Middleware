namespace KTI.Moo.Operation.Core.Domain.Reports;
public interface IOrder<GenericExtensionOrderModel, GenericBaseOrderModel> : IReport<GenericExtensionOrderModel, GenericBaseOrderModel> where GenericExtensionOrderModel : KTI.Moo.Extensions.Core.Model.OrderBase where GenericBaseOrderModel : KTI.Moo.Base.Model.OrderBase
{
}
