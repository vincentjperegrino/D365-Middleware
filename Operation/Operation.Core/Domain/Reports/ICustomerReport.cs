namespace KTI.Moo.Operation.Core.Domain.Reports;
public interface ICustomer<GenericExtensionCustomerModel, GenericBaseCustomerModel> : IReport<GenericExtensionCustomerModel, GenericBaseCustomerModel> where GenericExtensionCustomerModel : KTI.Moo.Extensions.Core.Model.CustomerBase where GenericBaseCustomerModel : KTI.Moo.Base.Model.CustomerBase
{

}
