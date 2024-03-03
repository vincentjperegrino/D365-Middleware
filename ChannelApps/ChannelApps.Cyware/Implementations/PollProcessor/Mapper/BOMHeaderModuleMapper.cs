using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class BOMHeaderModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<BillOfMaterialsHeader> bomHeader_d365 = JsonConvert.DeserializeObject<IEnumerable<BillOfMaterialsHeader>>(pollContent);
            var result = bomHeader_d365.Aggregate(
                new
                {
                    FormattedResults = new List<BOMHeaderPOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, bomheader) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.BOMHeader bomHeader_ = new Extensions.Cyware.Model.BOMHeader
                        {
                            BOMID = bomheader.BOMID ?? "",
                            MANUFACTUREDITEMNUMBER = bomheader.MANUFACTUREDITEMNUMBER ?? "",
                            PRODUCTIONSITEID = bomheader.PRODUCTIONSITEID ?? "",
                            PRODUCTCONFIGURATIONID = bomheader.PRODUCTCONFIGURATIONID ?? "",
                            PRODUCTSTYLEID = bomheader.PRODUCTSTYLEID ?? "",
                            ISACTIVE = bomheader.ISACTIVE ?? "",
                            FROMQUANTITY = bomheader.FROMQUANTITY,
                            VALIDFROMDATE = bomheader.validfromdate,
                            APPROVERPERSONNELNUMBER = bomheader.APPROVERPERSONNELNUMBER ?? "",
                            BOMNAME = bomheader.BOMNAME ?? "",
                            ISAPPROVED = bomheader.ISAPPROVED ?? ""
                        };

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new BOMHeaderPOLL(bomHeader_));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = bomheader.BOMID,
                            ErrorMessage = ex.Message
                        });
                        log.LogError($"{bomheader.BOMID}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}
