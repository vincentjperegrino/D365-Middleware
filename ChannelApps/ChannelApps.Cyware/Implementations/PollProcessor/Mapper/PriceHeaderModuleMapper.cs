using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class PriceHeaderModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<Prices> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<Prices>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<POLL64DTO>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.PriceHeader mapped = new Extensions.Cyware.Model.PriceHeader
                        {
                            evtNum = source.evtNum ?? "0",
                            evtDsc = source.evtDesc ?? "",
                            evtFdt = source.evtFdt ?? "",
                            evtTdt = source.evtTdt ?? ""
                        };

                        DateTime? startDate = mapped.evtFdt?.ToString() != null ? DateTime.TryParse(mapped.evtFdt.ToString(), out DateTime sDate) ? sDate : (DateTime?)null : null;
                        DateTime? endDate = mapped.evtFdt?.ToString() != null ? DateTime.TryParse(mapped.evtTdt.ToString(), out DateTime eDate) ? eDate : (DateTime?)null : null;

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new POLL64DTO(mapped.evtNum ?? "", mapped.evtDsc ?? "", startDate, endDate));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.evtNum,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.evtNum}: {ex.Message}");
                    }
                    return acc;
                });
            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}
