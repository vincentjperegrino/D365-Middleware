//using KTI.Moo.Extensions.Core.Service;

//namespace TestCyware.App
//{
//    public class POLLLOG : Model.TestBase
//    {
//        private readonly IBlobService blobService;
//        private readonly KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog pollLog;
//        public POLLLOG()
//        {
//            blobService = new KTI.Moo.Extensions.Cyware.Services.BlobService(config);
//            pollLog = new KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog(blobService);
//        }

//        [Fact]
//        public void Regular_Transaction_No_Discount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Regular_Transaction_No_Discount.SalesTransactionHeaders, Model.Data.Regular_Transaction_No_Discount.SalesTransactionLines, Model.Data.Regular_Transaction_No_Discount.SalesTransactionTenders, Model.Data.Regular_Transaction_No_Discount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Simple_Fixed_Amount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Simple_Fixed_Amount.SalesTransactionHeaders, Model.Data.Retail_Customer_Simple_Fixed_Amount.SalesTransactionLines, Model.Data.Retail_Customer_Simple_Fixed_Amount.SalesTransactionTenders, Model.Data.Retail_Customer_Simple_Fixed_Amount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Simple_Fixed_Percentage()
//        { 
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Simple_Fixed_Percentage.SalesTransactionHeaders, Model.Data.Retail_Customer_Simple_Fixed_Percentage.SalesTransactionLines, Model.Data.Retail_Customer_Simple_Fixed_Percentage.SalesTransactionTenders, Model.Data.Retail_Customer_Simple_Fixed_Percentage.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Qty_Based()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Qty_Based.SalesTransactionHeaders, Model.Data.Retail_Customer_Qty_Based.SalesTransactionLines, Model.Data.Retail_Customer_Qty_Based.SalesTransactionTenders, Model.Data.Retail_Customer_Qty_Based.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Time_Specific()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Time_Specific.SalesTransactionHeaders, Model.Data.Retail_Customer_Time_Specific.SalesTransactionLines, Model.Data.Retail_Customer_Time_Specific.SalesTransactionTenders, Model.Data.Retail_Customer_Time_Specific.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Total_Discount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Total_Discount.SalesTransactionHeaders, Model.Data.Retail_Customer_Total_Discount.SalesTransactionLines, Model.Data.Retail_Customer_Total_Discount.SalesTransactionTenders, Model.Data.Retail_Customer_Total_Discount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Tender_Type()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Tender_Type.SalesTransactionHeaders, Model.Data.Retail_Customer_Tender_Type.SalesTransactionLines, Model.Data.Retail_Customer_Tender_Type.SalesTransactionTenders, Model.Data.Retail_Customer_Tender_Type.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Retail_Customer_Coupon()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Retail_Customer_Coupon.SalesTransactionHeaders, Model.Data.Retail_Customer_Coupon.SalesTransactionLines, Model.Data.Retail_Customer_Coupon.SalesTransactionTenders, Model.Data.Retail_Customer_Coupon.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Family_Simple_Fixed_Amount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Family_Simple_Fixed_Amount.SalesTransactionHeaders, Model.Data.Family_Simple_Fixed_Amount.SalesTransactionLines, Model.Data.Family_Simple_Fixed_Amount.SalesTransactionTenders, Model.Data.Family_Simple_Fixed_Amount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Family_Simple_Fixed_Percentage()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Family_Simple_Fixed_Percentage.SalesTransactionHeaders, Model.Data.Family_Simple_Fixed_Percentage.SalesTransactionLines, Model.Data.Family_Simple_Fixed_Percentage.SalesTransactionTenders, Model.Data.Family_Simple_Fixed_Percentage.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Family_Total_Discount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Family_Total_Discount.SalesTransactionHeaders, Model.Data.Family_Total_Discount.SalesTransactionLines, Model.Data.Family_Total_Discount.SalesTransactionTenders, Model.Data.Family_Total_Discount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Employee_Simple_Fixed_Amount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Employee_Simple_Fixed_Amount.SalesTransactionHeaders, Model.Data.Employee_Simple_Fixed_Amount.SalesTransactionLines, Model.Data.Employee_Simple_Fixed_Amount.SalesTransactionTenders, Model.Data.Employee_Simple_Fixed_Amount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Employee_Simple_Fixed_Percentage()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Employee_Simple_Fixed_Percentage.SalesTransactionHeaders, Model.Data.Employee_Simple_Fixed_Percentage.SalesTransactionLines, Model.Data.Employee_Simple_Fixed_Percentage.SalesTransactionTenders, Model.Data.Employee_Simple_Fixed_Percentage.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Employee_Qty_Based()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Employee_Qty_Based.SalesTransactionHeaders, Model.Data.Employee_Qty_Based.SalesTransactionLines, Model.Data.Employee_Qty_Based.SalesTransactionTenders, Model.Data.Employee_Qty_Based.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Employee_Time_Specific()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Employee_Time_Specific.SalesTransactionHeaders, Model.Data.Employee_Time_Specific.SalesTransactionLines, Model.Data.Employee_Time_Specific.SalesTransactionTenders, Model.Data.Employee_Time_Specific.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }

//        [Fact]
//        public void Employee_Total_Discount()
//        {
//            bool result = pollLog.ProcessToQueue(Model.Data.Employee_Total_Discount.SalesTransactionHeaders, Model.Data.Employee_Total_Discount.SalesTransactionLines, Model.Data.Employee_Total_Discount.SalesTransactionTenders, Model.Data.Employee_Total_Discount.SalesTransactionDiscounts, config.AzureConnectionString);
//            Assert.True(result);
//        }
//    }
//}