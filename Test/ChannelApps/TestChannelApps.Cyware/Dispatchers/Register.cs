using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class Register : Model.BaseTest
    {
        private readonly IRegisterToQueue dispatcherToQueue;
        public Register()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Register();
        }

        [Fact]
        public void Test_Register_AppLayer()
        {
            // Arrange //
            //string myQueueItem = GetRegisterDispatcher();
            Registers d365_registers = new()
            {
                TerminalNumber = "T001",
                SelectHardwareStationOnTendering = "Yes",
                NumberSequenceGroup = "GroupA",
                AutoLogoffTimeout = 10,
                Name = "Sample Terminal",
                StatementMethod = "Print",
                ReturnInTransaction = "No",
                TerminalStatement = "Summary",
                ReceiptSetupLocation = "Main",
                HardwareProfile = "ProfileA",
                ReceiptPrintingDefaultOff = "No",
                OfflineDatabaseProfileName = "OfflineProfile",
                SupportOffline = "Yes",
                MaxReceiptTextLength = 100,
                LayoutId = "LayoutA",
                CustomerDisplayText1 = "Welcome!",
                OnlyTotalInSuspendedTransaction = "Yes",
                ReceiptBarcode = "12345",
                AutoExitMethod = "Timeout",
                PrintTaxRefundChecks = "No",
                PlaybackRecording = "Yes",
                OpenDrawerAtLastInLastOut = "Yes",
                ProductNumberOnReceipt = "No",
                ExitAfterEachTransaction = "No",
                ClosingStatus = "Closed",
                Location = "StoreA",
                CardNotPresentProcessingConfiguration = "ConfigA",
                NumberOfTopBottomLines = 5,
                ElectronicFundsTransferTenderTypeIdDefault = "TenderTypeA",
                StoreNumber = "12345",
                VisualProfile = "ProfileB",
                RetailTerminalOperationMode = "Normal",
                ManagerKeyOnReturn = "Yes",
                StandAlone = "No",
                SlipIfReturn = "Yes",
                ElectronicFundsTransferTerminal = "Yes",
                UpdateServicePort = 8080,
                DefaultDimensionLegalEntity = "EntityA",
                MaxDisplayTextLength = 80,
                CreateRecording = "No",
                StoreNumberForElectronicFundsTransfer = "54321",
                DefaultDimensionDisplayValue = "ValueA",
                CustomerDisplayText2 = "Thank you for shopping!",
                InternetProtocolAddress = "192.168.0.1",
                moosourcesystem = "SampleSystem",
                mooexternalid = "987654",
                companyid = 1
            };

            RegisterReading register = new()
            {
                LocationCode = d365_registers.Location,
                TransactionDate = 20220503,
                RegisterNum = int.Parse(d365_registers.StoreNumber),
                IsEod = 0,
                TransactionCount = 10,
                TransactionAmount = 100.50,
                TransactionNumber = 12345,
                IsSync = 1,
                TransNonVatAmount = 50.25,
                TransZeroRatedAmount = 25.00,
                TransVatAmount = 10.25,
                TransVatableAmount = 90.25,
                SeniorDiscount = 5.00M
            };
            string jsonString = JsonConvert.SerializeObject(register);

            // Act //
            bool result = Process(jsonString, registerQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}
