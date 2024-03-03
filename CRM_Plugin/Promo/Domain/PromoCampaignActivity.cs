using System;
using Microsoft.Xrm.Sdk;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using CRM_Plugin.Promo.Interface;
using Core_EntityHelper = CRM_Plugin.Core.Helper.EntityHelper;
using Helper = CRM_Plugin.Core.Helper.HashingAndEncryption;
using System.Collections;

namespace CRM_Plugin.Promo.Domain
{
    public class PromoCampaignActivity : IPromo
    {
        private readonly IOrganizationService service;
        private readonly ITracingService tracingService;
        ArrayList contactGuids = new ArrayList();

        public PromoCampaignActivity(IOrganizationService _service, ITracingService _tracingService)
        {
            service = _service;
            tracingService = _tracingService;
        }

        public bool Process(Entity _entity)
        {
            try
            {
                UpdateApprovalTime(_entity.ToEntityReference());

                var guidPromoCode = Helper.CreatePromoCodeTableFromCampaignActivity(_entity, service);

                if (guidPromoCode == Guid.Empty)
                    throw new Exception("Promo code not succesfully created");

                EntityReference erPromoCode = new EntityReference(Models.Marketing.PromoCode.entity_name, guidPromoCode);

                //this.LinkPromoCodeToCampaignActivityByER(_entity.ToEntityReference(), erPromoCode);

                var ecMarketingList = Helper.GetManytoManyRelationShip(_entity.Id, "list", "listid", "campaignactivitylist_association", "campaignactivity", "subject", service);

                foreach (var eMarketingList in ecMarketingList.Entities)
                {
                    contactGuids = Helper.GetAllMembersByMarketingList(contactGuids, eMarketingList.Id, service);
                }

                tracingService.Trace(contactGuids.Count.ToString());

                int numberRandomString = 0;
                bool uniquePromoCode = false;
                string promoCodePrefix = "";

                if (_entity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_promocodeprefix)))
                {
                    promoCodePrefix = (string)_entity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_promocodeprefix)];

                    if (_entity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_numberofrandomcharacters)))
                        numberRandomString = (int)_entity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_numberofrandomcharacters)];

                    if (_entity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_includesuniquepromocode)))
                        uniquePromoCode = (bool)_entity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_includesuniquepromocode)];
                }
                else
                {
                    promoCodePrefix = (string)_entity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.subject)];
                }

                Helper.CreatePromoCodeListTable(erPromoCode, promoCodePrefix, uniquePromoCode, numberRandomString, contactGuids, service);

                return true;
            }
            catch (Exception e)
            {
                tracingService.Trace(e.Message);

                return false;
            }
        }

        private void UpdateApprovalTime(EntityReference erCampaignActivity)
        {
            Entity updat_entity = new Entity(erCampaignActivity.LogicalName, erCampaignActivity.Id);

            updat_entity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_approvaltime)] = DateTime.UtcNow;

            service.Update(updat_entity);
        }

    }
}
