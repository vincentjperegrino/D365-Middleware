using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Domain;
using CRM_Plugin.CustomAPI.Model.DTO;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Domain
{
    public class SerialNumber : IOrderLine
    {
        IOrganizationService _service;
        ITracingService _tracingService;

        public SerialNumber(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }
        public bool upsert(Entity orderLine)
        {

            return false;
        }

        public bool create(Entity orderLine)
        {

            return false;
        }

        public bool update(Entity orderLine, Guid guid)
        {

            return false;
        }

        private Entity GetSerialNumberLineBySerialNumber(string serialNumber, string sku)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_orderlineserialnumber";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_serialnumber", ConditionOperator.Equal, serialNumber);
            entityFilter.AddCondition("kti_sku", ConditionOperator.Equal, sku);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Count > 0)
            {
                return returnEntityCollection.Entities.First();
            }

            return null;
        }

        private Guid CheckIfSerialLineExist(CustomAPI.Model.DTO.SerialNumbers serialNumber)
        {
            Entity serialLine = new Entity();

            serialLine = this.GetSerialNumberLineBySerialNumber(serialNumber.kti_serialnumber, serialNumber.kti_sku);

            if (serialLine != null)
                return serialLine.Id;

            return Guid.Empty;
        }

        public ExecuteMultipleResponse BulkUpsertSerialNumber(List<CustomAPI.Model.DTO.SerialNumbers> serialNumberList)
        {
            try
            {
                var multipleRequest = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                foreach (var entity in serialNumberList)
                {
                    Entity serialNumberEntity = new Entity("kti_orderlineserialnumber");
                    serialNumberEntity["kti_orderline"] = entity.kti_orderline;
                    serialNumberEntity["kti_parentbundleproduct"] = entity.kti_parentbundleproduct;
                    serialNumberEntity["kti_product"] = entity.kti_product;
                    serialNumberEntity["kti_serialnumber"] = entity.kti_serialnumber;
                    serialNumberEntity["kti_warrantystartdate"] = entity.kti_warrantystartdate;
                    serialNumberEntity["kti_sku"] = entity.kti_sku;

                    //Guid serialNumberID = this.CheckIfSerialLineExist(entity);

                    //if (Guid.Empty != serialNumberID)
                    //{
                    //    serialNumberEntity.Id = serialNumberID;

                    //    UpdateRequest updateRequest = new UpdateRequest() { Target = serialNumberEntity };
                    //    multipleRequest.Requests.Add(updateRequest);
                    //}
                    //else
                    //{
                        CreateRequest createRequest = new CreateRequest() { Target = serialNumberEntity };
                        multipleRequest.Requests.Add(createRequest);
                    //}
                }

                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)_service.Execute(multipleRequest);
                
                foreach (var r in multipleResponse.Responses)
                {
                    if (r.Fault != null)
                    {
                        _tracingService.Trace($"Order Line Serial Number Failed: {r.Fault.Message}");
                    }
                }

                return multipleResponse;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                return new ExecuteMultipleResponse();
            }
        }

        public Entity Get(Guid ExistingID)
        {
            throw new NotImplementedException();
        }

        public Entity Get(Guid ExistingSalesOrderID, string lineNumber)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetOrderItemList(Guid ExistingSalesOrderID)
        {
            throw new NotImplementedException();
        }

        public bool Upsert(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public Guid UpsertWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public bool Create(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public Guid CreateWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public bool Update(Guid SalesOrderID, Entity orderLine, Guid ExistingID)
        {
            throw new NotImplementedException();
        }
    }
}