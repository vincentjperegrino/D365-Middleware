using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class BOMVersionModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            //Deserialized to Moo Library DTOs
            IEnumerable<BOMVersions> content = JsonConvert.DeserializeObject<IEnumerable<BOMVersions>>(pollContent);
            var result = content.Aggregate(
                new
                {
                    FormattedResults = new List<BOMVersionPOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, _bomversion) =>
                {
                    try
                    {
                        ///Map to Extension's DTOs
                        Extensions.Cyware.Model.BOMVersion bomversion = new()
                        {
                            MANUFACTUREDITEMNUMBER = _bomversion.MANUFACTUREDITEMNUMBER ?? "",
                            BOMID = _bomversion.BOMID ?? "",
                            PRODUCTIONSITEID = _bomversion.PRODUCTSTYLEID ?? "",
                            PRODUCTCONFIGURATIONID = _bomversion.PRODUCTCONFIGURATIONID ?? "",
                            PRODUCTCOLORID = _bomversion.PRODUCTCOLORID ?? "",
                            PRODUCTSIZEID = _bomversion.PRODUCTSIZEID ?? "",
                            PRODUCTSTYLEID = _bomversion.PRODUCTSTYLEID ?? "",
                            PRODUCTVERSIONID = _bomversion.PRODUCTVERSIONID ?? "",
                            ISACTIVE = _bomversion.ISACTIVE ?? "",
                            VALIDFROMDATE = _bomversion.VALIDFROMDATE,
                            FROMQUANTITY = _bomversion.FROMQUANTITY,
                            SEQUENCEID = _bomversion.SEQUENCEID,
                            APPROVERPERSONNELNUMBER = _bomversion.APPROVERPERSONNELNUMBER ?? "",
                            CATCHWEIGHTSIZE = _bomversion.CATCHWEIGHTSIZE,
                            FROMCATCHWEIGHTQUANTITY = _bomversion.FROMCATCHWEIGHTQUANTITY,
                            ISAPPROVED = _bomversion.ISAPPROVED ?? "",
                            ISSELECTEDFORDESIGNER = _bomversion.ISSELECTEDFORDESIGNER ?? "",
                            VALIDTODATE = _bomversion.VALIDTODATE,
                            VERSIONNAME = _bomversion.VERSIONNAME ?? ""
                        };

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new BOMVersionPOLL(bomversion));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = _bomversion.BOMID,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{_bomversion.BOMID}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}
