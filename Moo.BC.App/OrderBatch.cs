using KTI.Moo.BC.App.Helpers;

namespace Moo.BC.App
{
    public class OrderBatch : Helpers.CompanySettings
    {
        [FunctionName("CRM-OrderBatch")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {

            log.LogInformation($"C# Customer Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(ConnectionString, $"{Companyid}-crm-order-batch");

            int companyid = Convert.ToInt32(Companyid);
             
            ReadFromAzureQueue(queueClient, companyid, log);

        }

        private bool ReadFromAzureQueue(QueueClient queueClient, int companyid, ILogger log, int maxmessage = 32)
        {
            string ErrorOn = "Receiving Messages in Queue:";
            try
            {

                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages(maxmessage);
                var TotalMessage = retrievedMessage.Length;

                if (TotalMessage > 0)
                {
                    ErrorOn = "Reading Messages in Queue:";
                    var ForInsertToOrderList = new List<Moo.BC.Model.DTO.Order>();
                    var OrderList = ReadFromCurrentQueueMessage(ForInsertToOrderList, retrievedMessage, log);

                    var Success_Transfer_FromMessages_ToList = OrderList is not null && OrderList.Count > 0;

                    if (Success_Transfer_FromMessages_ToList)
                    {
                        ErrorOn = "Posting to API:";
                        var SuccessPostToAPI = PostToAPI(ForInsertToOrderList, companyid, log);

                        if (SuccessPostToAPI)
                        {
                            ErrorOn = "Deleting Messages in Queue:";
                            var SuccessDeleteMessageInQueue = DeleteMessageInQueue(queueClient, retrievedMessage, log);

                            if (SuccessDeleteMessageInQueue)
                            {
                                log.LogInformation($"Popped {retrievedMessage.Length} Messages in Queue");
                                ReadFromAzureQueue(queueClient, companyid, log);
                            }

                        }

                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                log.LogError($"{ErrorOn} {ex.Message}");
                throw new Exception($"{ErrorOn} {ex.Message}");

            }

        }
        private bool DeleteMessageInQueue(QueueClient queueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
        {
            log.LogInformation($"Popping Messages in Queue");

            var CurrentMessage = retrievedMessages[currentcount];

            var TotalIteration = retrievedMessages.Length - 1;

            queueClient.DeleteMessage(CurrentMessage.MessageId, CurrentMessage.PopReceipt);

            if (TotalIteration > currentcount)
            {
                DeleteMessageInQueue(queueClient, retrievedMessages, log, ++currentcount);
            }

            return true;
        }

        private List<Moo.BC.Model.DTO.Order> ReadFromCurrentQueueMessage(List<Moo.BC.Model.DTO.Order> ForInsertToOrderList, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
        {
            var TotalIteration = retrievedMessages.Length - 1;
            var orders = Decode.Base64(retrievedMessages[currentcount].Body.ToString()).FromBrotliAsync().GetAwaiter().GetResult();

            var NewOrder = JsonConvert.DeserializeObject<Moo.BC.Model.DTO.Order>(orders);

            ForInsertToOrderList.Add(NewOrder);

            log.LogInformation($"Processing: {orders}");

            if (TotalIteration > currentcount)
            {
                ReadFromCurrentQueueMessage(ForInsertToOrderList, retrievedMessages, log, ++currentcount);
            }

            return ForInsertToOrderList;
        }

        private bool PostToAPI(List<Moo.BC.Model.DTO.Order> ForInsertToOrderList, int CompanyID, ILogger log)
        {

            //CRM.Domain.Order domain = new(CompanyID);

            //domain.upsertBatch(ForInsertToOrderList, log);

            return true;
        }
    }
}