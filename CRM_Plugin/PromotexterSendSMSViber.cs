using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web;

namespace CRM_Plugin
{
    public class PromotexterSendSMSViber : CodeActivity
    {
        ITracingService tracingService;
        IWorkflowContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        Entity ePromoCodeList, eCampaignActivity, ePromoCode;

        string mobileNumber = "",
            firstName = "",
            lastName = "",
            promoCode = "";

        string smsMessage = "";

        DateTime validFrom = DateTime.MinValue,
            validTo = DateTime.MinValue;

        [RequiredArgument]
        [ReferenceTarget("ncci_promocodelist")]
        [Input("Promo Code List")]
        public InArgument<EntityReference> inPromoCodeList { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var erPromoCodeList = this.inPromoCodeList.Get(executionContext);

            tracingService = executionContext.GetExtension<ITracingService>();
            context = executionContext.GetExtension<IWorkflowContext>();
            serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            if (service == null)
                tracingService.Trace("Service is empty and cannot continue the logic of workflow");

            ePromoCodeList = service.Retrieve(erPromoCodeList.LogicalName, erPromoCodeList.Id, new ColumnSet(true));

            ProcessPromotexterSendSMSViber();
        }

        private void ProcessPromotexterSendSMSViber()
        {
            try
            {
                if (ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_mobilenumber)) &&
                    ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_firstname)) &&
                    ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_lastname)) &&
                    ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_uniquepromocode)) &&
                    ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode)))
                {
                    firstName = (string)ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_firstname)];
                    lastName = (string)ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_lastname)];
                    mobileNumber = (string)ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_mobilenumber)];
                    promoCode = (string)ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_uniquepromocode)];

                    ePromoCode = GetPromoCode();

                    if (ePromoCode.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_campaignactivity)))
                    {
                        eCampaignActivity = GetCampaignActivity();

                        if (eCampaignActivity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.channeltypecode)))
                        {
                            int channelTypeCode = ((OptionSetValue)eCampaignActivity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.channeltypecode)]).Value;

                            if (channelTypeCode == 714430000 || channelTypeCode == 714430001)
                            {
                                if (eCampaignActivity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validfrom)) &&
                                    eCampaignActivity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validto)))
                                {
                                    validFrom = Helper.RetrieveLocalTimeFromUTCTime(service, (DateTime)eCampaignActivity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validfrom)]);
                                    validTo = Helper.RetrieveLocalTimeFromUTCTime(service, (DateTime)eCampaignActivity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validto)]);
                                }

                                if (eCampaignActivity.Contains(Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_smsmessage)))
                                {
                                    string response = "";

                                    smsMessage = (string)eCampaignActivity[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_smsmessage)];

                                    smsMessage = smsMessage.Replace("<LastName>", lastName);

                                    smsMessage = smsMessage.Replace("<FirstName>", firstName);

                                    smsMessage = smsMessage.Replace("<PromoCode>", promoCode);

                                    smsMessage = smsMessage.Replace("<ValidFrom>", validFrom.ToString("MMM dd, yyyy"));

                                    smsMessage = smsMessage.Replace("<ValidTo>", validTo.ToString("MMM dd, yyyy"));

                                    if (channelTypeCode == 714430000)
                                    {
                                        response = SendSMS().GetAwaiter().GetResult();
                                    }
                                    else
                                    {
                                        response = SendViberSMS().GetAwaiter().GetResult();
                                    }

                                    tracingService.Trace(response);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                tracingService.Trace(e.Message);
            }
        }

        private Entity GetPromoCode()
        {
            if (ePromoCodeList.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode)))
                return service.Retrieve(Models.Marketing.PromoCode.entity_name,
                    ((EntityReference)ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode)]).Id,
                    new ColumnSet(Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_campaignactivity)));

            return new Entity();
        }

        private Entity GetCampaignActivity()
        {
            if (ePromoCode.Contains(Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_campaignactivity)))
                return service.Retrieve(Models.Marketing.CampaignActivity.entity_name,
                    ((EntityReference)ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_campaignactivity)]).Id, new ColumnSet(true));

            return new Entity();
        }

        private async Task<string> SendSMS()
        {
            string apiKey = "0ad92e7674e5482b99dc887677c1c796";
            string apiSecret = "5d3ee39d799566806c770e74279df542";
            string fromName = "NespressoPH";

            string smsResponse = "";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Moo.Config.promotexter_base_url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await httpClient.PostAsync($"{Moo.Config.promotexter_sendsms_path}?apiKey={apiKey}&apiSecret={apiSecret}&from={fromName}&to={mobileNumber}&text={HttpUtility.UrlEncode(smsMessage)}", null);

                smsResponse = await response.Content.ReadAsStringAsync();
            }

            return smsResponse;
        }

        private async Task<string> SendViberSMS()
        {
            string apiKey = "0ad92e7674e5482b99dc887677c1c796";
            string apiSecret = "5d3ee39d799566806c770e74279df542";
            string viberID = "22692";

            string viberResponse = "";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Moo.Config.promotexter_base_url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await httpClient.PostAsync($"{Moo.Config.promotexter_sendviber_path}?apiKey={apiKey}&apiSecret={apiSecret}&from={viberID}&to={mobileNumber}&text={HttpUtility.UrlEncode(smsMessage)}", null);

                viberResponse = await response.Content.ReadAsStringAsync();
            }

            return viberResponse;
        }
    }
}