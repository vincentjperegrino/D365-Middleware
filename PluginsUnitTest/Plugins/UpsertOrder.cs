using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class UpsertOrder : TestBase
    {
        private readonly CRM_Plugin.UpsertOrder _Domain;
        private readonly ITracingService _tracingService;

        private EntityCollection orderCollection = new EntityCollection();
        private EntityCollection orderItemsCollection = new EntityCollection();
        private EntityCollection orderItemsSerialNumberCollection = new EntityCollection();
        private EntityCollection orderPaymentCollection = new EntityCollection();

        public UpsertOrder()
        {
            _service = connectToCRM();
            _Domain = new CRM_Plugin.UpsertOrder();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void UpsertOrderSpecialDiscount()
        {
            //Special Discount Type
            //959080000;VIP, 959080001;Senior, 959080002;PWD, 959080003; Other
            InitializeVariablesSpecialDiscount(959080001, "SAMPLETRANSACTIONDISCOUNTTYPE", Convert.ToDecimal(20.00));

            ParameterCollection parameterCollection = new ParameterCollection()
            {
                { new KeyValuePair<string, object>("kti_SyncOrdersCollection", orderCollection) },

                { new KeyValuePair<string, object>("kti_SyncOrderItemsCollection", orderItemsCollection) },

                { new KeyValuePair<string, object>("kti_SyncOrderItemsSerialNumberCollection", orderItemsSerialNumberCollection) },

                { new KeyValuePair<string, object>("kti_SyncPaymTransCollection", orderPaymentCollection) }
            };
            var response = _Domain.Process(tracingService, parameterCollection, _service);

            if (response.Entities.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public void UpsertOrderHappyPath()
        {
            InitializeVariablesHappyPath();

            ParameterCollection parameterCollection = new ParameterCollection()
            {
                { new KeyValuePair<string, object>("kti_SyncOrdersCollection", orderCollection) },

                { new KeyValuePair<string, object>("kti_SyncOrderItemsCollection", orderItemsCollection) },

                { new KeyValuePair<string, object>("kti_SyncOrderItemsSerialNumberCollection", orderItemsSerialNumberCollection) },

                { new KeyValuePair<string, object>("kti_SyncPaymTransCollection", orderPaymentCollection) }
            };
            var response = _Domain.Process(tracingService, parameterCollection, _service);

            if (response.Entities.Count > 0 )
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }

        private void InitializeVariablesSpecialDiscount(int specialDiscountType, string specialDiscountRefNo, decimal specialDiscountPercentage)
        {
            string staffID = "EMP-1042";
            string channelCode = "ZEAST";
            int channel = 959080013;
            DateTime datetimeNow = DateTime.Now;
            //string sourceID = $"{datetimeNow.TimeOfDay.TotalMilliseconds}_{channelCode}_{staffID}_{channel}";
            string sourceID = $"";

            #region Order header
            Entity orderEntity = new Entity();
            orderEntity.Id = Guid.Empty;
            orderEntity["name"] = sourceID;
            orderEntity["kti_specialdiscounttype"] = specialDiscountType;
            orderEntity["kti_specialdiscountrefno"] = specialDiscountRefNo;
            orderEntity["kti_socialchannelorigin"] = channel;//KiosKart
            orderEntity["kti_sourceid"] = sourceID;
            orderEntity["kti_channelcode"] = channelCode;
            orderEntity["kti_branchassigned"] = "Davao";
            orderEntity["kti_orderstatus"] = 959080006;//Delivered
            orderEntity["kti_staffid"] = staffID;
            orderEntity["kti_approvalmanager"] = "";
            orderEntity["billto_city"] = "Manila";
            orderEntity["billto_contactName"] = "John Doe";
            orderEntity["billto_country"] = "Philippines";
            orderEntity["billto_fax"] = "";
            orderEntity["billto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_name"] = "John Doe";
            orderEntity["billto_postalcode"] = "1008";
            orderEntity["emailaddress"] = "carlo.coroza@kationtechnologies.com";
            orderEntity["customerid"] = "19ff1c4b-07c0-ea11-a812-000d3a854028";
            orderEntity["description"] = "Sample description";
            orderEntity["discountamount"] = 0.00;
            orderEntity["discountpercentage"] = specialDiscountPercentage;
            orderEntity["freightamount"] = 0.00;
            orderEntity["overriddencreatedon"] = datetimeNow;
            orderEntity["pricelevelid"] = "Regular Price List";
            orderEntity["shipto_telephone"] = "09187509194";
            orderEntity["shipto_name"] = "John Doe";
            orderEntity["shipto_telephone"] = "John Doe";
            orderEntity["shipto_city"] = "Manila";
            orderEntity["shipto_contactName"] = "John Doe";
            orderEntity["shipto_country"] = "Philippines";
            orderEntity["shipto_fax"] = "";
            orderEntity["shipto_freighttermscode"] = 0;
            orderEntity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_postalcode"] = "1008";
            orderEntity["shipto_stateorprovince"] = "Metro Manila";
            orderEntity["transactioncurrencyid"] = "PHP";
            orderEntity["kti_channelurl"] = "";

            orderCollection.Entities.Add(orderEntity);
            #endregion

            #region Order lines
            Entity orderLinesEntity = new Entity();
            orderLinesEntity.Id = Guid.Empty;
            orderLinesEntity["salesorderid"] = sourceID;
            orderLinesEntity["kti_approvalmanager"] = "";
            orderLinesEntity["description"] = "";
            orderLinesEntity["extendedamount"] = 0.00;
            orderLinesEntity["isproductoverridden"] = false;
            orderLinesEntity["manualdiscountamount"] = 0.00;
            orderLinesEntity["priceperunit"] = 555.00;
            orderLinesEntity["overriddencreatedon"] = datetimeNow;
            orderLinesEntity["productid"] = "Jose Cuervox";
            orderLinesEntity["productdescription"] = "Jose Cuervo";
            orderLinesEntity["quantity"] = 3.00;
            orderLinesEntity["shipto_telephone"] = "09187509194";
            orderLinesEntity["shipto_name"] = "John Doe";
            orderLinesEntity["shipto_telephone"] = "John Doe";
            orderLinesEntity["shipto_city"] = "Manila";
            orderLinesEntity["shipto_contactName"] = "John Doe";
            orderLinesEntity["shipto_country"] = "Philippines";
            orderLinesEntity["shipto_fax"] = "";
            orderLinesEntity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_postalcode"] = "1008";
            orderLinesEntity["shipto_stateorprovince"] = "Metro Manila";
            orderLinesEntity["tax"] = 0.00;
            orderLinesEntity["transactioncurrencyid"] = "PHP";
            orderLinesEntity["kti_lineitemnumber"] = "1";
            orderLinesEntity["kti_sourceid"] = sourceID;
            orderLinesEntity["kti_socialchannelorigin"] = channel;

            orderItemsCollection.Entities.Add(orderLinesEntity);

            Entity orderLines1Entity = new Entity();
            orderLines1Entity.Id = Guid.Empty;
            orderLines1Entity["salesorderid"] = sourceID;
            orderLines1Entity["kti_approvalmanager"] = "";
            orderLines1Entity["description"] = "";
            orderLines1Entity["extendedamount"] = 0.00;
            orderLines1Entity["isproductoverridden"] = false;
            orderLines1Entity["manualdiscountamount"] = 0.00;
            orderLines1Entity["priceperunit"] = 1005.00;
            orderLines1Entity["overriddencreatedon"] = datetimeNow;
            orderLines1Entity["productid"] = "Tequila Package";
            orderLines1Entity["productdescription"] = "Tequila Package";
            orderLines1Entity["quantity"] = 2.00;
            orderLines1Entity["productname"] = "";
            orderLines1Entity["shipto_telephone"] = "09187509194";
            orderLines1Entity["shipto_name"] = "John Doe";
            orderLines1Entity["shipto_telephone"] = "John Doe";
            orderLines1Entity["shipto_city"] = "Manila";
            orderLines1Entity["shipto_contactName"] = "John Doe";
            orderLines1Entity["shipto_country"] = "Philippines";
            orderLines1Entity["shipto_fax"] = "";
            orderLines1Entity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_postalcode"] = "1008";
            orderLines1Entity["shipto_stateorprovince"] = "Metro Manila";
            orderLines1Entity["tax"] = 0.00;
            orderLines1Entity["transactioncurrencyid"] = "PHP";
            orderLines1Entity["kti_lineitemnumber"] = "2";
            orderLines1Entity["kti_sourceid"] = sourceID;
            orderLines1Entity["kti_socialchannelorigin"] = channel;

            orderItemsCollection.Entities.Add(orderLines1Entity);
            #endregion

            #region Order Serial Number
            //3 Jose Cuervo
            Entity serialNumberJoseCuervoEntity = new Entity();
            serialNumberJoseCuervoEntity.Id = Guid.Empty;
            serialNumberJoseCuervoEntity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervoEntity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervoEntity["serialnumber"] = "4215637";
            serialNumberJoseCuervoEntity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervoEntity);

            Entity serialNumberJoseCuervo1Entity = new Entity();
            serialNumberJoseCuervo1Entity.Id = Guid.Empty;
            serialNumberJoseCuervo1Entity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervo1Entity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervo1Entity["serialnumber"] = "51432523";
            serialNumberJoseCuervo1Entity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervo1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervo1Entity);

            Entity serialNumberJoseCuervo2Entity = new Entity();
            serialNumberJoseCuervo2Entity.Id = Guid.Empty;
            serialNumberJoseCuervo2Entity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervo2Entity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervo2Entity["serialnumber"] = "123456123";
            serialNumberJoseCuervo2Entity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervo2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervo2Entity);

            //2 Tequila Package = 2 Jose Cuervo and 3 Mojito

            //2 Tequila Package = 2 Jose Cuervo = 4 Jose Cuervo
            Entity serialNumberTPJoseCuervoEntity = new Entity();
            serialNumberTPJoseCuervoEntity.Id = Guid.Empty;
            serialNumberTPJoseCuervoEntity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervoEntity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervoEntity["serialnumber"] = "1523451234";
            serialNumberTPJoseCuervoEntity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervoEntity);

            Entity serialNumberTPJoseCuervo1Entity = new Entity();
            serialNumberTPJoseCuervo1Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo1Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo1Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo1Entity["serialnumber"] = "234667235";
            serialNumberTPJoseCuervo1Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo1Entity);

            Entity serialNumberTPJoseCuervo2Entity = new Entity();
            serialNumberTPJoseCuervo2Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo2Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo2Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo2Entity["serialnumber"] = "1523445124";
            serialNumberTPJoseCuervo2Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo2Entity);

            Entity serialNumberTPJoseCuervo3Entity = new Entity();
            serialNumberTPJoseCuervo3Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo3Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo3Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo3Entity["serialnumber"] = "12341261234123";
            serialNumberTPJoseCuervo3Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo3Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo3Entity);


            //2 Tequila Package = 3 Mojito = 6 Mojito
            Entity serialNumberTPMojitoEntity = new Entity();
            serialNumberTPMojitoEntity.Id = Guid.Empty;
            serialNumberTPMojitoEntity["kti_lineitemnumber"] = "2";
            serialNumberTPMojitoEntity["kti_sourceid"] = sourceID;
            serialNumberTPMojitoEntity["serialnumber"] = "1562341265";
            serialNumberTPMojitoEntity["productid"] = "Mojito";
            serialNumberTPMojitoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojitoEntity);

            Entity serialNumberTPMojito1Entity = new Entity();
            serialNumberTPMojito1Entity.Id = Guid.Empty;
            serialNumberTPMojito1Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito1Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito1Entity["serialnumber"] = "45236723451";
            serialNumberTPMojito1Entity["productid"] = "Mojito";
            serialNumberTPMojito1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito1Entity);

            Entity serialNumberTPMojito2Entity = new Entity();
            serialNumberTPMojito2Entity.Id = Guid.Empty;
            serialNumberTPMojito2Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito2Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito2Entity["serialnumber"] = "1234512631234";
            serialNumberTPMojito2Entity["productid"] = "Mojito";
            serialNumberTPMojito2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito2Entity);

            Entity serialNumberTPMojito3Entity = new Entity();
            serialNumberTPMojito3Entity.Id = Guid.Empty;
            serialNumberTPMojito3Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito3Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito3Entity["serialnumber"] = "5123412512341";
            serialNumberTPMojito3Entity["productid"] = "Mojito";
            serialNumberTPMojito3Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito3Entity);

            Entity serialNumberTPMojito4Entity = new Entity();
            serialNumberTPMojito4Entity.Id = Guid.Empty;
            serialNumberTPMojito4Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito4Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito4Entity["serialnumber"] = "1235123412345";
            serialNumberTPMojito4Entity["productid"] = "Mojito";
            serialNumberTPMojito4Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito4Entity);

            Entity serialNumberTPMojito5Entity = new Entity();
            serialNumberTPMojito5Entity.Id = Guid.Empty;
            serialNumberTPMojito5Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito5Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito5Entity["serialnumber"] = "4123561234123";
            serialNumberTPMojito5Entity["productid"] = "Mojito";
            serialNumberTPMojito5Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito5Entity);
            #endregion

            #region Payment
            //Multiple payments
            //3675

            //Cash
            Entity paymentLinesCashEntity = new Entity();
            paymentLinesCashEntity.Id = Guid.Empty;
            paymentLinesCashEntity["kti_socialchannelorigin"] = channel;
            paymentLinesCashEntity["kti_sourceid"] = sourceID;
            paymentLinesCashEntity["kti_transactionid"] = "";
            paymentLinesCashEntity["kti_merchantname"] = "";
            paymentLinesCashEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesCashEntity["kti_paymentmethod"] = 959080002;
            paymentLinesCashEntity["kti_amount"] = 1000.00;

            orderPaymentCollection.Entities.Add(paymentLinesCashEntity);

            //Home Credit
            Entity paymentLinesHomeCreditEntity = new Entity();
            paymentLinesHomeCreditEntity.Id = Guid.Empty;
            paymentLinesHomeCreditEntity["kti_socialchannelorigin"] = channel;
            paymentLinesHomeCreditEntity["kti_sourceid"] = sourceID;
            paymentLinesHomeCreditEntity["kti_transactionid"] = "14234123-41234-12";
            paymentLinesHomeCreditEntity["kti_merchantname"] = "";
            paymentLinesHomeCreditEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesHomeCreditEntity["kti_paymentmethod"] = 959080044;
            paymentLinesHomeCreditEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesHomeCreditEntity);

            //Grab Pay
            Entity paymentLinesGrabPayEntity = new Entity();
            paymentLinesGrabPayEntity.Id = Guid.Empty;
            paymentLinesGrabPayEntity["kti_socialchannelorigin"] = channel;
            paymentLinesGrabPayEntity["kti_sourceid"] = sourceID;
            paymentLinesGrabPayEntity["kti_transactionid"] = "5243-52-345-234";
            paymentLinesGrabPayEntity["kti_merchantname"] = "";
            paymentLinesGrabPayEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesGrabPayEntity["kti_paymentmethod"] = 959080045;
            paymentLinesGrabPayEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesHomeCreditEntity);

            //GCASH
            Entity paymentLinesGCASHEntity = new Entity();
            paymentLinesGCASHEntity.Id = Guid.Empty;
            paymentLinesGCASHEntity["kti_socialchannelorigin"] = channel;
            paymentLinesGCASHEntity["kti_sourceid"] = sourceID;
            paymentLinesGCASHEntity["kti_transactionid"] = "1234-1234-1423-";
            paymentLinesGCASHEntity["kti_merchantname"] = "";
            paymentLinesGCASHEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesGCASHEntity["kti_paymentmethod"] = 959080000;
            paymentLinesGCASHEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesGCASHEntity);

            //Paymaya
            Entity paymentLinesMayaEntity = new Entity();
            paymentLinesMayaEntity.Id = Guid.Empty;
            paymentLinesMayaEntity["kti_socialchannelorigin"] = channel;
            paymentLinesMayaEntity["kti_sourceid"] = sourceID;
            paymentLinesMayaEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesMayaEntity["kti_merchantname"] = "";
            paymentLinesMayaEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesMayaEntity["kti_paymentmethod"] = 959080003;
            paymentLinesMayaEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesMayaEntity);

            //Debit/Credit Card
            Entity paymentLinesDebitCreditCardEntity = new Entity();
            paymentLinesDebitCreditCardEntity.Id = Guid.Empty;
            paymentLinesDebitCreditCardEntity["kti_socialchannelorigin"] = channel;
            paymentLinesDebitCreditCardEntity["kti_sourceid"] = sourceID;
            paymentLinesDebitCreditCardEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesDebitCreditCardEntity["kti_merchantname"] = "UnionBank";
            paymentLinesDebitCreditCardEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesDebitCreditCardEntity["kti_paymentmethod"] = 959080011;
            paymentLinesDebitCreditCardEntity["kti_amount"] = 1200.00;

            orderPaymentCollection.Entities.Add(paymentLinesDebitCreditCardEntity);

            //Cheque
            Entity paymentLinesChequeEntity = new Entity();
            paymentLinesChequeEntity.Id = Guid.Empty;
            paymentLinesChequeEntity["kti_socialchannelorigin"] = channel;
            paymentLinesChequeEntity["kti_sourceid"] = sourceID;
            paymentLinesChequeEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesChequeEntity["kti_merchantname"] = "";
            paymentLinesChequeEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesChequeEntity["kti_paymentmethod"] = 959080040;
            paymentLinesChequeEntity["kti_amount"] = 675.00;

            orderPaymentCollection.Entities.Add(paymentLinesChequeEntity);
            #endregion
        }
        private void InitializeVariablesCoupon()
        {
            Entity orderEntity = new Entity();
            orderEntity.Id = Guid.Empty;
            orderEntity["kti_specialdiscounttype"] = 0;//
            orderEntity["kti_specialdiscountrefno"] = "";
            orderCollection.Entities.Add(orderEntity);
        }
        private void InitializeVariablesHappyPath()
        {
            string staffID = "EMP-1042";
            string channelCode = "ZEAST";
            int channel = 959080013;
            DateTime datetimeNow = DateTime.Now;
            //string sourceID = $"{datetimeNow.TimeOfDay.TotalMilliseconds}_{channelCode}_{staffID}_{channel}";
            string sourceID = $"";

            #region Order header
            Entity orderEntity = new Entity();
            orderEntity.Id = Guid.Empty;
            orderEntity["name"] = sourceID;
            orderEntity["kti_specialdiscounttype"] = 0;
            orderEntity["kti_specialdiscountrefno"] = "";
            orderEntity["kti_socialchannelorigin"] = channel;//KiosKart
            orderEntity["kti_sourceid"] = sourceID;
            orderEntity["kti_channelcode"] = channelCode;
            orderEntity["kti_branchassigned"] = "Davao";
            orderEntity["kti_orderstatus"] = 959080006;//Delivered
            orderEntity["kti_staffid"] = staffID;
            orderEntity["kti_approvalmanager"] = "";
            orderEntity["billto_city"] = "Manila";
            orderEntity["billto_contactName"] = "John Doe";
            orderEntity["billto_country"] = "Philippines";
            orderEntity["billto_fax"] = "";
            orderEntity["billto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["billto_name"] = "John Doe";
            orderEntity["billto_postalcode"] = "1008";
            orderEntity["emailaddress"] = "carlo.coroza@kationtechnologies.com";
            orderEntity["customerid"] = "19ff1c4b-07c0-ea11-a812-000d3a854028";
            orderEntity["description"] = "Sample description";
            orderEntity["discountamount"] = 0.00;
            orderEntity["discountpercentage"] = 0.00;
            orderEntity["freightamount"] = 0.00;
            orderEntity["overriddencreatedon"] = datetimeNow;
            orderEntity["pricelevelid"] = "Regular Price List";
            orderEntity["shipto_telephone"] = "09187509194";
            orderEntity["shipto_name"] = "John Doe";
            orderEntity["shipto_telephone"] = "John Doe";
            orderEntity["shipto_city"] = "Manila";
            orderEntity["shipto_contactName"] = "John Doe";
            orderEntity["shipto_country"] = "Philippines";
            orderEntity["shipto_fax"] = "";
            orderEntity["shipto_freighttermscode"] = 0;
            orderEntity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderEntity["shipto_postalcode"] = "1008";
            orderEntity["shipto_stateorprovince"] = "Metro Manila";
            orderEntity["transactioncurrencyid"] = "PHP";
            orderEntity["kti_channelurl"] = "";

            orderCollection.Entities.Add(orderEntity);
            #endregion

            #region Order lines
            Entity orderLinesEntity = new Entity();
            orderLinesEntity.Id = Guid.Empty;
            orderLinesEntity["salesorderid"] = sourceID;
            orderLinesEntity["kti_approvalmanager"] = "";
            orderLinesEntity["description"] = "";
            orderLinesEntity["extendedamount"] = 0.00;
            orderLinesEntity["isproductoverridden"] = false;
            orderLinesEntity["manualdiscountamount"] = 0.00;
            orderLinesEntity["priceperunit"] = 555.00;
            orderLinesEntity["overriddencreatedon"] = datetimeNow;
            orderLinesEntity["productid"] = "Jose Cuervox";
            orderLinesEntity["productdescription"] = "Jose Cuervo";
            orderLinesEntity["quantity"] = 3.00;
            orderLinesEntity["shipto_telephone"] = "09187509194";
            orderLinesEntity["shipto_name"] = "John Doe";
            orderLinesEntity["shipto_telephone"] = "John Doe";
            orderLinesEntity["shipto_city"] = "Manila";
            orderLinesEntity["shipto_contactName"] = "John Doe";
            orderLinesEntity["shipto_country"] = "Philippines";
            orderLinesEntity["shipto_fax"] = "";
            orderLinesEntity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderLinesEntity["shipto_postalcode"] = "1008";
            orderLinesEntity["shipto_stateorprovince"] = "Metro Manila";
            orderLinesEntity["tax"] = 0.00;
            orderLinesEntity["transactioncurrencyid"] = "PHP";
            orderLinesEntity["kti_lineitemnumber"] = "1";
            orderLinesEntity["kti_sourceid"] = sourceID;
            orderLinesEntity["kti_socialchannelorigin"] = channel;

            orderItemsCollection.Entities.Add(orderLinesEntity);

            Entity orderLines1Entity = new Entity();
            orderLines1Entity.Id = Guid.Empty;
            orderLines1Entity["salesorderid"] = sourceID;
            orderLines1Entity["kti_approvalmanager"] = "";
            orderLines1Entity["description"] = "";
            orderLines1Entity["extendedamount"] = 0.00;
            orderLines1Entity["isproductoverridden"] = false;
            orderLines1Entity["manualdiscountamount"] = 0.00;
            orderLines1Entity["priceperunit"] = 1005.00;
            orderLines1Entity["overriddencreatedon"] = datetimeNow;
            orderLines1Entity["productid"] = "Tequila Package";
            orderLines1Entity["productdescription"] = "Tequila Package";
            orderLines1Entity["quantity"] = 2.00;
            orderLines1Entity["productname"] = "";
            orderLines1Entity["shipto_telephone"] = "09187509194";
            orderLines1Entity["shipto_name"] = "John Doe";
            orderLines1Entity["shipto_telephone"] = "John Doe";
            orderLines1Entity["shipto_city"] = "Manila";
            orderLines1Entity["shipto_contactName"] = "John Doe";
            orderLines1Entity["shipto_country"] = "Philippines";
            orderLines1Entity["shipto_fax"] = "";
            orderLines1Entity["shipto_line1"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_line2"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_line3"] = "2453 Isagani St., Sta. Cruz";
            orderLines1Entity["shipto_postalcode"] = "1008";
            orderLines1Entity["shipto_stateorprovince"] = "Metro Manila";
            orderLines1Entity["tax"] = 0.00;
            orderLines1Entity["transactioncurrencyid"] = "PHP";
            orderLines1Entity["kti_lineitemnumber"] = "2";
            orderLines1Entity["kti_sourceid"] = sourceID;
            orderLines1Entity["kti_socialchannelorigin"] = channel;

            orderItemsCollection.Entities.Add(orderLines1Entity);
            #endregion

            #region Order Serial Number
            //3 Jose Cuervo
            Entity serialNumberJoseCuervoEntity = new Entity();
            serialNumberJoseCuervoEntity.Id = Guid.Empty;
            serialNumberJoseCuervoEntity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervoEntity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervoEntity["serialnumber"] = "4215637";
            serialNumberJoseCuervoEntity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervoEntity);

            Entity serialNumberJoseCuervo1Entity = new Entity();
            serialNumberJoseCuervo1Entity.Id = Guid.Empty;
            serialNumberJoseCuervo1Entity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervo1Entity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervo1Entity["serialnumber"] = "51432523";
            serialNumberJoseCuervo1Entity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervo1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervo1Entity);

            Entity serialNumberJoseCuervo2Entity = new Entity();
            serialNumberJoseCuervo2Entity.Id = Guid.Empty;
            serialNumberJoseCuervo2Entity["kti_lineitemnumber"] = "1";
            serialNumberJoseCuervo2Entity["kti_sourceid"] = sourceID;
            serialNumberJoseCuervo2Entity["serialnumber"] = "123456123";
            serialNumberJoseCuervo2Entity["productid"] = "Jose Cuervo";
            serialNumberJoseCuervo2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberJoseCuervo2Entity);

            //2 Tequila Package = 2 Jose Cuervo and 3 Mojito

            //2 Tequila Package = 2 Jose Cuervo = 4 Jose Cuervo
            Entity serialNumberTPJoseCuervoEntity = new Entity();
            serialNumberTPJoseCuervoEntity.Id = Guid.Empty;
            serialNumberTPJoseCuervoEntity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervoEntity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervoEntity["serialnumber"] = "1523451234";
            serialNumberTPJoseCuervoEntity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervoEntity);

            Entity serialNumberTPJoseCuervo1Entity = new Entity();
            serialNumberTPJoseCuervo1Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo1Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo1Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo1Entity["serialnumber"] = "234667235";
            serialNumberTPJoseCuervo1Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo1Entity);

            Entity serialNumberTPJoseCuervo2Entity = new Entity();
            serialNumberTPJoseCuervo2Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo2Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo2Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo2Entity["serialnumber"] = "1523445124";
            serialNumberTPJoseCuervo2Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo2Entity);

            Entity serialNumberTPJoseCuervo3Entity = new Entity();
            serialNumberTPJoseCuervo3Entity.Id = Guid.Empty;
            serialNumberTPJoseCuervo3Entity["kti_lineitemnumber"] = "1";
            serialNumberTPJoseCuervo3Entity["kti_sourceid"] = sourceID;
            serialNumberTPJoseCuervo3Entity["serialnumber"] = "12341261234123";
            serialNumberTPJoseCuervo3Entity["productid"] = "Jose Cuervo";
            serialNumberTPJoseCuervo3Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPJoseCuervo3Entity);


            //2 Tequila Package = 3 Mojito = 6 Mojito
            Entity serialNumberTPMojitoEntity = new Entity();
            serialNumberTPMojitoEntity.Id = Guid.Empty;
            serialNumberTPMojitoEntity["kti_lineitemnumber"] = "2";
            serialNumberTPMojitoEntity["kti_sourceid"] = sourceID;
            serialNumberTPMojitoEntity["serialnumber"] = "1562341265";
            serialNumberTPMojitoEntity["productid"] = "Mojito";
            serialNumberTPMojitoEntity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojitoEntity);

            Entity serialNumberTPMojito1Entity = new Entity();
            serialNumberTPMojito1Entity.Id = Guid.Empty;
            serialNumberTPMojito1Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito1Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito1Entity["serialnumber"] = "45236723451";
            serialNumberTPMojito1Entity["productid"] = "Mojito";
            serialNumberTPMojito1Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito1Entity);

            Entity serialNumberTPMojito2Entity = new Entity();
            serialNumberTPMojito2Entity.Id = Guid.Empty;
            serialNumberTPMojito2Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito2Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito2Entity["serialnumber"] = "1234512631234";
            serialNumberTPMojito2Entity["productid"] = "Mojito";
            serialNumberTPMojito2Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito2Entity);

            Entity serialNumberTPMojito3Entity = new Entity();
            serialNumberTPMojito3Entity.Id = Guid.Empty;
            serialNumberTPMojito3Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito3Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito3Entity["serialnumber"] = "5123412512341";
            serialNumberTPMojito3Entity["productid"] = "Mojito";
            serialNumberTPMojito3Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito3Entity);

            Entity serialNumberTPMojito4Entity = new Entity();
            serialNumberTPMojito4Entity.Id = Guid.Empty;
            serialNumberTPMojito4Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito4Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito4Entity["serialnumber"] = "1235123412345";
            serialNumberTPMojito4Entity["productid"] = "Mojito";
            serialNumberTPMojito4Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito4Entity);

            Entity serialNumberTPMojito5Entity = new Entity();
            serialNumberTPMojito5Entity.Id = Guid.Empty;
            serialNumberTPMojito5Entity["kti_lineitemnumber"] = "2";
            serialNumberTPMojito5Entity["kti_sourceid"] = sourceID;
            serialNumberTPMojito5Entity["serialnumber"] = "4123561234123";
            serialNumberTPMojito5Entity["productid"] = "Mojito";
            serialNumberTPMojito5Entity["kti_socialchannelorigin"] = channel;

            orderItemsSerialNumberCollection.Entities.Add(serialNumberTPMojito5Entity);
            #endregion

            #region Payment
            //Multiple payments
            //3675

            //Cash
            Entity paymentLinesCashEntity = new Entity();
            paymentLinesCashEntity.Id = Guid.Empty;
            paymentLinesCashEntity["kti_socialchannelorigin"] = channel;
            paymentLinesCashEntity["kti_sourceid"] = sourceID;
            paymentLinesCashEntity["kti_transactionid"] = "";
            paymentLinesCashEntity["kti_merchantname"] = "";
            paymentLinesCashEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesCashEntity["kti_paymentmethod"] = 959080002;
            paymentLinesCashEntity["kti_amount"] = 1000.00;

            orderPaymentCollection.Entities.Add(paymentLinesCashEntity);

            //Home Credit
            Entity paymentLinesHomeCreditEntity = new Entity();
            paymentLinesHomeCreditEntity.Id = Guid.Empty;
            paymentLinesHomeCreditEntity["kti_socialchannelorigin"] = channel;
            paymentLinesHomeCreditEntity["kti_sourceid"] = sourceID;
            paymentLinesHomeCreditEntity["kti_transactionid"] = "14234123-41234-12";
            paymentLinesHomeCreditEntity["kti_merchantname"] = "";
            paymentLinesHomeCreditEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesHomeCreditEntity["kti_paymentmethod"] = 959080044;
            paymentLinesHomeCreditEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesHomeCreditEntity);

            //Grab Pay
            Entity paymentLinesGrabPayEntity = new Entity();
            paymentLinesGrabPayEntity.Id = Guid.Empty;
            paymentLinesGrabPayEntity["kti_socialchannelorigin"] = channel;
            paymentLinesGrabPayEntity["kti_sourceid"] = sourceID;
            paymentLinesGrabPayEntity["kti_transactionid"] = "5243-52-345-234";
            paymentLinesGrabPayEntity["kti_merchantname"] = "";
            paymentLinesGrabPayEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesGrabPayEntity["kti_paymentmethod"] = 959080045;
            paymentLinesGrabPayEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesHomeCreditEntity);

            //GCASH
            Entity paymentLinesGCASHEntity = new Entity();
            paymentLinesGCASHEntity.Id = Guid.Empty;
            paymentLinesGCASHEntity["kti_socialchannelorigin"] = channel;
            paymentLinesGCASHEntity["kti_sourceid"] = sourceID;
            paymentLinesGCASHEntity["kti_transactionid"] = "1234-1234-1423-";
            paymentLinesGCASHEntity["kti_merchantname"] = "";
            paymentLinesGCASHEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesGCASHEntity["kti_paymentmethod"] = 959080000;
            paymentLinesGCASHEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesGCASHEntity);

            //Paymaya
            Entity paymentLinesMayaEntity = new Entity();
            paymentLinesMayaEntity.Id = Guid.Empty;
            paymentLinesMayaEntity["kti_socialchannelorigin"] = channel;
            paymentLinesMayaEntity["kti_sourceid"] = sourceID;
            paymentLinesMayaEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesMayaEntity["kti_merchantname"] = "";
            paymentLinesMayaEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesMayaEntity["kti_paymentmethod"] = 959080003;
            paymentLinesMayaEntity["kti_amount"] = 200.00;

            orderPaymentCollection.Entities.Add(paymentLinesMayaEntity);

            //Debit/Credit Card
            Entity paymentLinesDebitCreditCardEntity = new Entity();
            paymentLinesDebitCreditCardEntity.Id = Guid.Empty;
            paymentLinesDebitCreditCardEntity["kti_socialchannelorigin"] = channel;
            paymentLinesDebitCreditCardEntity["kti_sourceid"] = sourceID;
            paymentLinesDebitCreditCardEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesDebitCreditCardEntity["kti_merchantname"] = "UnionBank";
            paymentLinesDebitCreditCardEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesDebitCreditCardEntity["kti_paymentmethod"] = 959080011;
            paymentLinesDebitCreditCardEntity["kti_amount"] = 1200.00;

            orderPaymentCollection.Entities.Add(paymentLinesDebitCreditCardEntity);

            //Cheque
            Entity paymentLinesChequeEntity = new Entity();
            paymentLinesChequeEntity.Id = Guid.Empty;
            paymentLinesChequeEntity["kti_socialchannelorigin"] = channel;
            paymentLinesChequeEntity["kti_sourceid"] = sourceID;
            paymentLinesChequeEntity["kti_transactionid"] = "12356-1234123";
            paymentLinesChequeEntity["kti_merchantname"] = "";
            paymentLinesChequeEntity["kti_paymentdate"] = datetimeNow;
            paymentLinesChequeEntity["kti_paymentmethod"] = 959080040;
            paymentLinesChequeEntity["kti_amount"] = 675.00;

            orderPaymentCollection.Entities.Add(paymentLinesChequeEntity);
            #endregion
        }

    }
}
