using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.Amqp.Framing;
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
    public class BOMLinesModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<BillOfMaterialsDetails> bomHeader_d365 = JsonConvert.DeserializeObject<IEnumerable<BillOfMaterialsDetails>>(pollContent);
            var result = bomHeader_d365.Aggregate(
                new
                {
                    FormattedResults = new List<BOMLinesPOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, _bomLines) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.BOMLines bomLines_ = new Extensions.Cyware.Model.BOMLines
                        {
                            BOMID = _bomLines.BOMID ?? "",
                            LINECREATIONSEQUENCENUMBER = _bomLines.LINECREATIONSEQUENCENUMBER,
                            CATCHWEIGHTQUANTITY = _bomLines.CATCHWEIGHTQUANTITY,
                            CONFIGURATIONGROUPID = _bomLines.CONFIGURATIONGROUPID ?? "",
                            CONSTANTSCRAPQUANTITY = _bomLines.CONSTANTSCRAPQUANTITY,
                            CONSUMPTIONCALCULATIONCONSTANT = _bomLines.CONSUMPTIONCALCULATIONCONSTANT,
                            CONSUMPTIONCALCULATIONMETHOD = _bomLines.CONSUMPTIONCALCULATIONMETHOD ?? "",
                            CONSUMPTIONSITEID = _bomLines.CONSUMPTIONSITEID ?? "",
                            CONSUMPTIONTYPE = _bomLines.CONSUMPTIONTYPE ?? "",
                            CONSUMPTIONWAREHOUSEID = _bomLines.CONSUMPTIONWAREHOUSEID ?? "",
                            FLUSHINGPRINCIPLE = _bomLines.FLUSHINGPRINCIPLE ?? "",
                            ISCONSUMEDATOPERATIONCOMPLETE = _bomLines.ISCONSUMEDATOPERATIONCOMPLETE ?? "",
                            ISRESOURCECONSUMPTIONUSED = _bomLines.ISRESOURCECONSUMPTIONUSED ?? "",
                            ITEMNUMBER = _bomLines.ITEMNUMBER ?? "",
                            LINENUMBER = _bomLines.LINENUMBER,
                            LINETYPE = _bomLines.LINETYPE ?? "",
                            PHYSICALPRODUCTDENSITY = _bomLines.PHYSICALPRODUCTDENSITY,
                            PHYSICALPRODUCTDEPTH = _bomLines.PHYSICALPRODUCTDEPTH,
                            PHYSICALPRODUCTHEIGHT = _bomLines.PHYSICALPRODUCTHEIGHT,
                            PHYSICALPRODUCTWIDTH = _bomLines.PHYSICALPRODUCTWIDTH,
                            POSITIONNUMBER = _bomLines.POSITIONNUMBER ?? "",
                            PRODUCTCOLORID = _bomLines.PRODUCTCOLORID ?? "",
                            PRODUCTCONFIGURATIONID = _bomLines.PRODUCTCONFIGURATIONID ?? "",
                            PRODUCTSIZEID = _bomLines.PRODUCTSIZEID ?? "",
                            PRODUCTSTYLEID = _bomLines.PRODUCTSTYLEID ?? "",
                            PRODUCTUNITSYMBOL = _bomLines.PRODUCTUNITSYMBOL ?? "",
                            PRODUCTVERSIONID = _bomLines.PRODUCTVERSIONID ?? "",
                            QUANTITY = _bomLines.quantity,
                            QUANTITYDENOMINATOR = _bomLines.QUANTITYDENOMINATOR,
                            QUANTITYROUNDINGUPMULTIPLES = _bomLines.QUANTITYROUNDINGUPMULTIPLES,
                            ROUNDINGUPMETHOD = _bomLines.ROUNDINGUPMETHOD ?? "",
                            ROUTEOPERATIONNUMBER = _bomLines.ROUTEOPERATIONNUMBER,
                            SUBBOMID = _bomLines.SUBBOMID ?? "",
                            SUBROUTEID = _bomLines.SUBROUTEID ?? "",
                            VALIDFROMDATE = _bomLines.VALIDFROMDATE,
                            VALIDTODATE = _bomLines.VALIDTODATE,
                            VARIABLESCRAPPERCENTAGE = _bomLines.VARIABLESCRAPPERCENTAGE,
                            VENDORACCOUNTNUMBER = _bomLines.VENDORACCOUNTNUMBER ?? "",
                            WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE = _bomLines.WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE ?? "",
                            WILLCOSTCALCULATIONINCLUDELINE = _bomLines.WILLCOSTCALCULATIONINCLUDELINE ?? "",
                            WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES = _bomLines.WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES ?? "",
                            WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES = _bomLines.WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES ?? ""
                        };

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new BOMLinesPOLL(bomLines_));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = _bomLines.BOMID,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{_bomLines.BOMID}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }


        private static void Validate(Extensions.Cyware.Model.BOMLines products)
        {

            if (string.IsNullOrEmpty(products.CONFIGURATIONGROUPID))
            {
                throw new ArgumentException("BOM Lines Configuration Group is required.");
            }
        }
    }
}
