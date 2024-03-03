using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Customer;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class CustomerModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to Extension's DTOs
            IEnumerable<DTO_Customer> customer_d365 = JsonConvert.DeserializeObject<IEnumerable<DTO_Customer>>(pollContent);
            var result = customer_d365.Aggregate(
                new
                {
                    FormattedResults = new List<CustomerPOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.Customer customer = new Extensions.Cyware.Model.Customer
                        {
                            CustomerId = source.CustomerAccount ?? "",
                            CurrencyCode = source.SalesCurrencyCode ?? "",
                            LocationCode = source.location_code ?? "",
                            firstname = (source.first_name ?? "").Substring(0, Math.Min((source.first_name ?? "").Length, 30)), // Trim to 30 Characters
                            middlename = !string.IsNullOrEmpty(source.middle_initial) ? source.middle_initial.TrimStart().Substring(0, 1) : "",
                            lastname = (source.last_name ?? "").Substring(0, Math.Min((source.last_name ?? "").Length, 30)), // Trim to 30 Characters
                            CompanyName = source.company_name ?? "",
                            Remarks = source.soremarks ?? "",
                            NameAlias = (source.NameAlias ?? "").Substring(0, Math.Min((source.NameAlias ?? "").Length, 30)), // Trim to 30 Characters
                            birthdate = source.birthday ?? "",
                            PrimaryContactEmail = source.email ?? "",
                            ContactNumber = source.cellphone ?? "",
                            Name = (source.OrganizationName ?? "").Substring(0, Math.Min((source.OrganizationName ?? "").Length, 30)), // Trim to 30 Characters
                            Type = source.PartyType ?? "",
                            CustomerGroup = source.CustomerGroupId ?? "",
                            PriceGroup = source.DiscountPriceGroupId ?? ""
                        };

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new CustomerPOLL(customer));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.CustomerAccount,
                            ErrorMessage = ex.Message
                        });
                        log.LogError($"{source.CustomerAccount}: {ex.Message}");
                    }
                    return acc;
                });
            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}


