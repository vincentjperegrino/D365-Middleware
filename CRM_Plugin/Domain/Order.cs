using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Domain
{
    public class Order : IOrder
    {
        IOrganizationService _service;
        ITracingService _tracingService;

        public Order(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }
        public bool Upsert(Entity order)
        {

            return false;
        }

        public bool Create(Entity order)
        {

            return false;
        }

        public bool Update(Entity order, Guid guid)
        {

            return false;
        }

        public string GenerateOrderNumber(int channel, DateTime createdOn, string branchCode = null, string staffID = null)
        {
            string orderNumber = "";
            string prefixOrderNumber = "";

            switch (channel)
            {
                //Kiosk
                case var value when value == CRM_Plugin.OptionSet.Payment.KiosKart:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.KiosKart;
                    break;

                //Lazada
                case var value when value == CRM_Plugin.OptionSet.Payment.Lazada:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.Lazada;
                    break;

                //SAP
                case var value when value == CRM_Plugin.OptionSet.Payment.SAP:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.SAP;
                    break;

                //Website
                case var value when value == CRM_Plugin.OptionSet.Payment.Website:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.Website;
                    break;

                //Octopus
                case var value when value == CRM_Plugin.OptionSet.Payment.OctoPOS:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.OctoPOS;
                    break;

                case var value when value == CRM_Plugin.OptionSet.Payment.Magento:
                    prefixOrderNumber = CRM_Plugin.Values.ChannelOrderNumberPrefix.Magento;
                    break;
            }

            if (Int32.TryParse(GetLastOrderNumberByChannel(prefixOrderNumber, channel, createdOn), out int numSeq))
            {
                numSeq += 1;
            }

            orderNumber = $"{prefixOrderNumber}-{createdOn.Year}-{numSeq.ToString()}";

            if ((!String.IsNullOrEmpty(staffID) && !String.IsNullOrEmpty(branchCode)) &&
                channel == CRM_Plugin.OptionSet.Payment.KiosKart)
                orderNumber = $"{orderNumber}-{branchCode}-{staffID}";

            return orderNumber;
        }

        private string GetLastOrderNumberByChannel(string prefixOrderNumber, int channel, DateTime createdOn)
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            qeEntity.ColumnSet = new ColumnSet("name");

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("name", ConditionOperator.BeginsWith, prefixOrderNumber);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin, ConditionOperator.Equal, channel);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnOrder = _service.RetrieveMultiple(qeEntity);

            if (returnOrder.Entities.Any())
            {
                var listOrder = returnOrder.Entities.ToList();

                //listOrder = listOrder.OrderByDescending(o => (string)o["name"]).ToList();

                if (listOrder.Count > 0)
                {
                    return listOrder.Select(o => int.Parse(((string)o["name"]).Split('-')[2])).Max().ToString();
                }
            }

            return "0";
        }
        public Entity Get(string sourceid, string channelid)
        {
            if (sourceid == null || channelid == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_socialchannelorigin, ConditionOperator.Equal, Convert.ToInt32(channelid));

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnOrder = _service.RetrieveMultiple(qeEntity);

            if (returnOrder.Entities.Count > 0)
            {
                return returnOrder.Entities.First();
            }

            return null;
        }

        private Guid CheckIfOrderHeaderExist(CustomAPI.Model.DTO.Orders order)
        {
            var _order = this.Get(order.kti_sourceid, order.kti_socialchannelorigin.ToString());

            if (_order != null)
                return _order.Id;

            return Guid.Empty;
        }

        public EntityCollection BulkUpsertOrders(List<CustomAPI.Model.DTO.Orders> ordersList)
        {
            try
            {
                EntityCollection successCollection = new EntityCollection();

                var multipleRequest = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                foreach (var entity in ordersList)
                {
                    Entity orderEntity = new Entity("salesorder");

                    orderEntity["name"] = entity.name;
                    orderEntity["billto_city"] = entity.billto_city;
                    orderEntity["billto_contactname"] = entity.billto_contactName;
                    orderEntity["billto_country"] = entity.billto_country;
                    orderEntity["billto_fax"] = entity.billto_fax;
                    orderEntity["billto_line1"] = entity.billto_line1;
                    orderEntity["billto_line2"] = entity.billto_line2;
                    orderEntity["billto_line3"] = entity.billto_line3;
                    orderEntity["billto_name"] = entity.billto_name;
                    orderEntity["billto_postalcode"] = entity.billto_postalcode;
                    orderEntity["billto_stateorprovince"] = entity.billto_stateorprovince;
                    orderEntity["billto_telephone"] = entity.billto_telephone;
                    orderEntity["kti_socialchannelorigin"] = new OptionSetValue(entity.kti_socialchannelorigin);
                    orderEntity["kti_sourceid"] = entity.kti_sourceid;
                    orderEntity["shipto_name"] = entity.shipto_name;
                    orderEntity["emailaddress"] = entity.emailaddress;
                    orderEntity["shipto_telephone"] = entity.shipto_telephone;
                    orderEntity["description"] = entity.description;
                    orderEntity["discountamount"] = entity.discountamount;
                    orderEntity["discountpercentage"] = entity.discountpercentage;
                    orderEntity["freightamount"] = entity.freightamount;
                    orderEntity["overriddencreatedon"] = entity.overriddencreatedon;
                    orderEntity["shipto_city"] = entity.shipto_city;
                    orderEntity["shipto_contactname"] = entity.shipto_contactName;
                    orderEntity["shipto_country"] = entity.shipto_country;
                    orderEntity["shipto_fax"] = entity.shipto_fax;
                    orderEntity["shipto_line1"] = entity.shipto_line1;
                    orderEntity["shipto_line2"] = entity.shipto_line2;
                    orderEntity["shipto_line3"] = entity.shipto_line3;
                    orderEntity["shipto_postalcode"] = entity.shipto_postalcode;
                    orderEntity["shipto_stateorprovince"] = entity.shipto_stateorprovince;
                    orderEntity["totalamount"] = entity.totalamount;
                    orderEntity["totalamountlessfreight"] = entity.totalamountlessfreight;
                    orderEntity["totaldiscountamount"] = entity.totaldiscountamount;
                    orderEntity["totallineitemamount"] = entity.totallineitemamount;
                    orderEntity["totallineitemdiscountamount"] = entity.totallineitemdiscountamount;
                    orderEntity["totaltax"] = entity.totaltax;
                    orderEntity["kti_orderstatus"] = entity.kti_orderstatus;
                    orderEntity["kti_channelurl"] = entity.kti_channelurl;
                    orderEntity["kti_orderstatus"] = new OptionSetValue(entity.kti_orderstatus);
                    orderEntity["transactioncurrencyid"] = new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.Currency.entity_name, Guid.Parse(entity.transactioncurrencyid));
                    orderEntity["customerid"] = entity.customer;
                    orderEntity["pricelevelid"] = new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name, Guid.Parse(entity.pricelevelid));
                    orderEntity["kti_branchassigned"] = new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.Branch.entity_name, Guid.Parse(entity.branch_assigned));

                    var orderID = CheckIfOrderHeaderExist(entity);

                    if (Guid.Empty != orderID)
                    {
                        orderEntity.Id = orderID;

                        UpdateRequest updateRequest = new UpdateRequest() { Target = orderEntity };
                        multipleRequest.Requests.Add(updateRequest);
                    }
                    else
                    {
                        CreateRequest createRequest = new CreateRequest() { Target = orderEntity };
                        multipleRequest.Requests.Add(createRequest);
                    }
                }

                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)_service.Execute(multipleRequest);

                foreach (var r in multipleResponse.Responses)
                {
                    if (r.Fault != null)
                    {
                        _tracingService.Trace($"Order Failed: {r.Fault.Message}");
                    }
                    else
                    {
                        Entity response = new Entity();
                        response["order_id"] = r.Response["id"];

                        successCollection.Entities.Add(response);
                    }
                }

                return successCollection;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);
                return new EntityCollection();
            }
        }

        public Entity Get(Guid ExistingID)
        {
            throw new NotImplementedException();
        }

        public Guid UpsertWithGuid(Entity order)
        {
            throw new NotImplementedException();
        }

        public Guid CreateWithGuid(Entity order)
        {
            throw new NotImplementedException();
        }

        public Entity Get(string sourceid, OptionSetValue channel)
        {
            throw new NotImplementedException();
        }
    }
}
