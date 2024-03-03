using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections;
using System.Security.Cryptography;

namespace CRM_Plugin
{
    public static class Helper
    {

        public static Entity GetEntityByStringAttribute(string criteriaValue, EntityCollection theEntityCollection, string attributeName)
        {
            IEnumerable<Entity> entity = from tempEntity in theEntityCollection.Entities.AsEnumerable()
                                         where tempEntity.Contains(attributeName)
                                         && (string)tempEntity[attributeName] == criteriaValue
                                         select tempEntity;

            return entity.ToList().First();
        }


        public static EntityCollection GetCRMEntity(string entity, IOrganizationService service)
        {
            var qe = new QueryExpression();
            qe.EntityName = entity;
            qe.ColumnSet = new ColumnSet(true);

            EntityCollection ecEntity = service.RetrieveMultiple(qe);

            return ecEntity;
        }

        public static EntityCollection GetActiveEntityCollectionByName(string entityName, ColumnSet columnSet, IOrganizationService service)
        {
            if (!(string.IsNullOrEmpty(entityName) && columnSet != null))
                throw new Exception("Empty entity name or columnset");

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = entityName;
            qeEntity.ColumnSet = columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("statecode", ConditionOperator.Equal, 1);

            qeEntity.Criteria.AddFilter(entityFilter);

            return service.RetrieveMultiple(qeEntity);
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }
        public static EntityCollection GetManytoManyRelationShip(Guid entityBGuid, string offenceEntityName, string offenceColumnSet, string relationShipName, string legalEntityName, string legalColumnSet, IOrganizationService _service)
        {
            QueryExpression query = new QueryExpression(offenceEntityName);
            ColumnSet cols = new ColumnSet();
            cols.AddColumn(offenceColumnSet);
            cols.AddColumn("listname");
            query.ColumnSet = cols;

            Relationship relationship = new Relationship(relationShipName);
            RelationshipQueryCollection relationshipColl = new RelationshipQueryCollection();
            relationshipColl.Add(relationship, query);

            RetrieveRequest request = new RetrieveRequest();
            request.RelatedEntitiesQuery = relationshipColl;
            request.Target = new EntityReference(legalEntityName, entityBGuid);
            request.ColumnSet = new ColumnSet(legalColumnSet);
            RetrieveResponse response = (RetrieveResponse)_service.Execute(request);

            return response.Entity.RelatedEntities[relationship];
        }

        public static ArrayList GetAllMembersByMarketingList(ArrayList contactGuids, Guid marketingListId, IOrganizationService service)
        {
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.Count = 5000;
            pageInfo.PageNumber = 1;

            QueryByAttribute query = new QueryByAttribute("listmember");
            // pass the guid of the Static marketing list
            query.AddAttributeValue("listid", marketingListId);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection entityCollection = service.RetrieveMultiple(query);

            foreach (Entity entity in entityCollection.Entities)
            {
                contactGuids.Add(((EntityReference)entity.Attributes["entityid"]).Id);
            }

            // if list contains more than 5000 records
            while (entityCollection.MoreRecords)
            {
                query.PageInfo.PageNumber += 1;
                query.PageInfo.PagingCookie = entityCollection.PagingCookie;
                entityCollection = service.RetrieveMultiple(query);

                foreach (Entity entity in entityCollection.Entities)
                {
                    contactGuids.Add(((EntityReference)entity.Attributes["entityid"]).Id);
                }
            }

            return contactGuids;
        }

        public static bool CheckIfPromoCodeHasExistingRecord(EntityReference erPromoCode, string promoCode, IOrganizationService service)
        {
            var query = new QueryExpression
            {
                NoLock = true,
                TopCount = 1,
                EntityName = Models.Marketing.PromoCodeList.entity_name,
                ColumnSet = new ColumnSet(false)
            };

            query.Criteria.AddCondition(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_uniquepromocode), ConditionOperator.Equal, promoCode);

            query.Criteria.AddCondition(Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode), ConditionOperator.Equal, erPromoCode.Id);

            var entities = service.RetrieveMultiple(query);

            if (entities.Entities.Count == 0)
            {
                return false;
            }

            return true;
        }
        public static string RandomString(int length)
        {
            string valid = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    sb.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            return sb.ToString();
        }

        public static void CreatePromoCodeListTable(EntityReference erPromoCode, string promoCodePrefix, bool uniquePromoCode, int numberRandomString, ArrayList contactGuids, IOrganizationService service)
        {
            // Create an ExecuteMultipleRequest object.
            var multipleRequest = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            int countThousand = 0;
            int countContact = 0;
            int totalContactGuid = contactGuids.Count;

            foreach (Guid contactGuid in contactGuids)
            {
                string promoCode = promoCodePrefix;

                if(contactGuid != Guid.Empty || contactGuid != null)
                {
                    string firstName = promoCode;
                    string lastName = promoCode;
                    string mobileNumber = "";

                    Entity eContact = service.Retrieve("contact", contactGuid, new ColumnSet("firstname", "lastname", "mobilephone"));

                    if (eContact.Contains("firstname"))
                        firstName = (string)eContact["firstname"];

                    if (eContact.Contains("lastname"))
                        lastName = (string)eContact["lastname"];

                    if (eContact.Contains("mobilephone"))
                        mobileNumber = (string)eContact["mobilephone"];

                    if (uniquePromoCode)
                    {
                        bool notUnique = true;

                        while (notUnique)
                        {
                            promoCode = $"{promoCodePrefix}{Helper.RandomString(numberRandomString)}";

                            notUnique = Helper.CheckIfPromoCodeHasExistingRecord(erPromoCode, promoCode, service);
                        }
                    }

                    Entity ePromoCodeList = new Entity(Models.Marketing.PromoCodeList.entity_name);

                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode)] = erPromoCode;
                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_uniquepromocode)] = promoCode;
                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_customer)] = new EntityReference("contact", contactGuid);
                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_firstname)] = firstName;
                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_lastname)] = lastName;
                    ePromoCodeList[Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_mobilenumber)] = mobileNumber;

                    CreateRequest createRequest = new CreateRequest { Target = ePromoCodeList };

                    multipleRequest.Requests.Add(createRequest);

                    countThousand += 1;
                    countContact += 1;

                    if (countThousand == 1000 || countContact == totalContactGuid)
                    {
                        // Execute all the requests in the request collection using a single web method call.
                        ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);

                        if(countThousand == 1000)
                        {
                            multipleRequest = new ExecuteMultipleRequest()
                            {
                                Settings = new ExecuteMultipleSettings()
                                {
                                    ContinueOnError = true,
                                    ReturnResponses = true
                                },

                                Requests = new OrganizationRequestCollection()
                            };

                            countThousand = 0;
                        }
                    }
                }
            }
        }

        public static Guid CreatePromoCodeTableFromCampaignActivity(Entity eCampaignAcitivty, IOrganizationService service)
        {
            string colPromoCode = Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_promocodeprefix);
            string colValidFrom = Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validfrom);
            string colValidTo = Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_validto);
            string colNumRandomString = Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.ncci_numberofrandomcharacters);
            string colSubject = Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.subject);

            var ePromoCode = new Entity(Models.Marketing.PromoCode.entity_name);

            ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_campaignactivity)] = eCampaignAcitivty.ToEntityReference();

            if (eCampaignAcitivty.Contains(colPromoCode)
                && eCampaignAcitivty.Contains(colValidFrom)
                && eCampaignAcitivty.Contains(colValidTo))
            {
                ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_promocode)] = eCampaignAcitivty[colPromoCode];
                ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_validfrom)] = eCampaignAcitivty[colValidFrom];
                ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_validto)] = eCampaignAcitivty[colValidTo];
            }
            else
            {
                ePromoCode[Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_promocode)] = eCampaignAcitivty[colSubject];
            }

            return service.Create(ePromoCode);
        }

        public static DateTime RetrieveLocalTimeFromUTCTime(IOrganizationService service, DateTime utcTime)
        {
            return RetrieveLocalTimeFromUTCTime(utcTime, RetrieveCurrentUsersSettings(service), service);
        }

        public static DateTime RetrieveUTCTimeFromLocalTime(IOrganizationService service, DateTime localTime)
        {
            return RetrieveUTCTimeFromLocalTime(localTime, RetrieveCurrentUsersSettings(service), service);
        }

        public static int? RetrieveCurrentUsersSettings(IOrganizationService service)
        {
            var currentUserSettings = service.RetrieveMultiple(
                new QueryExpression("usersettings")
                {
                    ColumnSet = new ColumnSet("timezonecode"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                    new ConditionExpression("systemuserid", ConditionOperator.EqualUserId)
                        }
                    }
                }).Entities[0].ToEntity<Entity>();
            return (int?)currentUserSettings.Attributes["timezonecode"];
        }

        public static DateTime RetrieveLocalTimeFromUTCTime(DateTime utcTime, int? timeZoneCode, IOrganizationService service)
        {
            if (!timeZoneCode.HasValue)
                return DateTime.Now;
            var request = new LocalTimeFromUtcTimeRequest
            {
                TimeZoneCode = timeZoneCode.Value,
                UtcTime = utcTime.ToUniversalTime()
            };
            var response = (LocalTimeFromUtcTimeResponse)service.Execute(request);
            return response.LocalTime;
        }

        public static DateTime RetrieveUTCTimeFromLocalTime(DateTime localTime, int? timeZoneCode, IOrganizationService service)
        {
            if (!timeZoneCode.HasValue)
                return DateTime.Now;
            var request = new UtcTimeFromLocalTimeRequest
            {
                TimeZoneCode = timeZoneCode.Value,
                LocalTime = localTime
            };
            var response = (UtcTimeFromLocalTimeResponse)service.Execute(request);
            return response.UtcTime;
        }
    }
}
