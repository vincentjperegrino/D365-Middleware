using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CRM_Plugin.CustomAPI.Model.DTO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin
{
    public class UpsertOrder: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService)); //for tracinglog, for tracing
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext)); //providing info on what action/event of data, holding parameter
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory)); //initialization of authorazation of instance, crud process
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId); //CRM library
            try
            {
                var output = Process(tracingService, context.InputParameters, service);

                if(output.Entities.Any())
                {
                    context.OutputParameters[" "] = output;
                }
            }
            catch (Exception e)
            {
                tracingService.Trace(e.Message);

                throw new Exception(e.Message);
            }
        }

        private static bool ValidateOrdersParam(ParameterCollection customParams)
        {
            if (customParams.Contains(""))
            {
                throw new Exception("Error: Order parameter is missing.");
            }

            return true;
        }

        public EntityCollection Process(ITracingService tracingService, ParameterCollection customParams, IOrganizationService service)
        {
            var ordersList = new List<Orders>();
            var orderLineList = new List<CRM_Plugin.CustomAPI.Model.DTO.OrderDetails>();
            var serialNumberList = new List<CRM_Plugin.CustomAPI.Model.DTO.SerialNumbers>();
            var paymTransList = new List<CRM_Plugin.CustomAPI.Model.DTO.PaymentTransaction>();

            // return entitycollection response
            EntityCollection responseCol = new EntityCollection();

            var responseList = new List<Entity>();
            bool isOrderParamPresent = ValidateOrdersParam(customParams);

            if (isOrderParamPresent)
            {
                // for orders
                if (customParams.Contains("kti_SyncOrdersCollection"))
                {
                    var ordersCol = new EntityCollection { EntityName = "Microsoft.Dynamics.CRM.expando" };
                    ordersCol = (EntityCollection)customParams["kti_SyncOrdersCollection"];

                    if (ordersCol != null && ordersCol.Entities.Count > 0)
                    {
                        ordersList = ordersCol.Entities.Select(ol => new Orders(ol, service, tracingService)).ToList();

                        var orderDomain = new CRM_Plugin.Domain.Order(service, tracingService);

                        responseCol = orderDomain.BulkUpsertOrders(ordersList);
                    }
                }

                // for order line
                if (customParams.Contains("kti_SyncOrderItemsCollection"))
                {
                    var orderLineListCol = new EntityCollection { EntityName = "Microsoft.Dynamics.CRM.expando" };
                    orderLineListCol = (EntityCollection)customParams["kti_SyncOrderItemsCollection"];

                    if (orderLineListCol != null && orderLineListCol.Entities.Count > 0)
                    {
                        orderLineList = orderLineListCol.Entities.Select(ol => new CRM_Plugin.CustomAPI.Model.DTO.OrderDetails(ordersList.First(),
                            ol, service, tracingService)).ToList();

                        var orderListDomain = new CRM_Plugin.Domain.OrderLine(service, tracingService);

                        orderListDomain.BulkUpsertOrderItems(orderLineList);
                    }
                }

                // for order line serial number
                if (customParams.Contains("kti_SyncOrderItemsSerialNumberCollection"))
                {
                    var orderListSerialNumberCol = new EntityCollection { EntityName = "Microsoft.Dynamics.CRM.expando" };
                    orderListSerialNumberCol = (EntityCollection)customParams["kti_SyncOrderItemsSerialNumberCollection"];

                    if (orderListSerialNumberCol != null && orderListSerialNumberCol.Entities.Count > 0)
                    {
                        serialNumberList = orderListSerialNumberCol.Entities.Select(serial => new CRM_Plugin.CustomAPI.Model.DTO.SerialNumbers(ordersList.First(),
                            serial, service, tracingService)).ToList();

                        var serialNumberDomain = new CRM_Plugin.Domain.SerialNumber(service, tracingService);

                        serialNumberDomain.BulkUpsertSerialNumber(serialNumberList);
                    }
                }

                // for order payment
                if (customParams.Contains("kti_SyncPaymTransCollection"))
                {
                    var orderPaymentListCol = new EntityCollection { EntityName = "Microsoft.Dynamics.CRM.expando" };
                    orderPaymentListCol = (EntityCollection)customParams["kti_SyncPaymTransCollection"];

                    if (orderPaymentListCol != null && orderPaymentListCol.Entities.Count > 0)
                    {
                        paymTransList = orderPaymentListCol.Entities.Select(paym => new CRM_Plugin.CustomAPI.Model.DTO.PaymentTransaction(ordersList.First(),
                            paym, service, tracingService)).ToList();

                        var paymTransDomain = new CRM_Plugin.Domain.PaymentTransaction(service, tracingService);

                        paymTransDomain.BulkUpsertPaymentTrans(paymTransList);
                    }
                }
            }

            return responseCol;
        }
    }
}
