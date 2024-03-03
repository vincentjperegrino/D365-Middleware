using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;
using KTI.Moo.Extensions.Cyware.Model;
using Moo.Models.Dtos.Items;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Register
    {
        private readonly IRegisterToQueue _dispatcherToQueue;
        private readonly string _queueName = "moo-register-queue";
        private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private readonly string _companyID = Environment.GetEnvironmentVariable("CompanyID");

        public Register(IRegisterToQueue dispatcherToQueue)
        {
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Register")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Register queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
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
                Process(jsonString, _queueName, _connectionString, _companyID);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}
