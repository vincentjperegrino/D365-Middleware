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
    internal class PaymentTransaction : IPaymentTransaction
    {
        IOrganizationService _service;
        ITracingService _tracingService;

        public PaymentTransaction(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }
        public bool upsert(Entity paymentTrans)
        {

            return false;
        }

        public bool create(Entity paymentTrans)
        {

            return false;
        }

        public bool update(Entity paymentTrans, Guid guid)
        {

            return false;
        }

        private Entity GetPaymentTransactionByTransactionIDSourceIDPaymentMethod(string transactionID, Guid sourceID, int paymentMethod)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_paymenttransaction";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_transactionid", ConditionOperator.Equal, transactionID);
            entityFilter.AddCondition("kti_order", ConditionOperator.Equal, sourceID);
            entityFilter.AddCondition("kti_paymentmethod", ConditionOperator.Equal, paymentMethod);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Count > 0)
            {
                return returnEntityCollection.Entities.First();
            }

            return null;
        }

        private Guid CheckIfPaymentTransactionExist(CustomAPI.Model.DTO.PaymentTransaction paymentTransaction)
        {
            Entity _paymentTransaction = new Entity();

            Entity _order = _service.Retrieve("salesorder", paymentTransaction.kti_order.Id, new ColumnSet(true));

            if (_order.Contains("kti_sourceid"))
            {
                _paymentTransaction = this.GetPaymentTransactionByTransactionIDSourceIDPaymentMethod(paymentTransaction.kti_transactionid, _order.Id, paymentTransaction.kti_paymentmethod.Value);
            }

            if (_paymentTransaction != null)
                return _paymentTransaction.Id;

            return Guid.Empty;
        }

        public ExecuteMultipleResponse BulkUpsertPaymentTrans(List<CustomAPI.Model.DTO.PaymentTransaction> paymTransList)
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

                foreach (var entity in paymTransList)
                {
                    Entity paymTransEntity = new Entity("kti_paymenttransaction");
                    paymTransEntity["kti_amount"] = entity.kti_amount;
                    paymTransEntity["kti_transactionid"] = entity.kti_transactionid;
                    paymTransEntity["kti_authenticationid"] = entity.kti_authenticationid;
                    paymTransEntity["kti_authorizationid"] = entity.kti_authorizationid; 
                    paymTransEntity["kti_chargingid"] = entity.kti_chargingid;
                    paymTransEntity["emailaddress"] = entity.emailaddress;
                    paymTransEntity["kti_merchantname"] = entity.kti_merchantname;
                    paymTransEntity["transactioncurrencyid"] = entity.transactioncurrencyid;
                    paymTransEntity["kti_paymentdate"] = entity.kti_paymentdate;
                    paymTransEntity["kti_paymentmethod"] = entity.kti_paymentmethod;
                    paymTransEntity["kti_reason"] = entity.kti_reason;

                    if(entity.kti_invoice != null)
                        paymTransEntity["kti_invoice"] = entity.kti_invoice;

                    if (entity.kti_order != null)
                        paymTransEntity["kti_order"] = entity.kti_order;

                    if (entity.kti_paymentstatus == null)
                    {
                        paymTransEntity["kti_paymentstatus"] = new OptionSetValue(CRM_Plugin.OptionSet.PaymentStatus.Paid);
                    }
                    else
                    {
                        paymTransEntity["kti_paymentstatus"] = entity.kti_paymentstatus;
                    }

                    Guid paymTransID = this.CheckIfPaymentTransactionExist(entity);

                    if (Guid.Empty != paymTransID)
                    {
                        paymTransEntity.Id = paymTransID;

                        UpdateRequest updateRequest = new UpdateRequest() { Target = paymTransEntity };
                        multipleRequest.Requests.Add(updateRequest);
                    }
                    else
                    {
                        CreateRequest createRequest = new CreateRequest() { Target = paymTransEntity };
                        multipleRequest.Requests.Add(createRequest);
                    }
                }

                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)_service.Execute(multipleRequest);
                foreach (var r in multipleResponse.Responses)
                {
                    if (r.Fault != null)
                    {
                        _tracingService.Trace($"Payment Transaction Failed: {r.Fault.Message}");
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
    }
}