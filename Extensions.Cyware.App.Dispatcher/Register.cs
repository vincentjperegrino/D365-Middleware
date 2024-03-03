﻿//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
//using System.IO;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

//namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
//{
//    public class Register
//    {
//        private readonly IRegisterToQueue _dispatcherToQueue;
//        private readonly string _queueName = "cyware-register-queue";
//        private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
//        private static string _companyID = Environment.GetEnvironmentVariable("CompanyID");
//        private readonly SFTPService.Services.SFTPService sftpService = new(_companyID);
//        private readonly string _rootFolder = Environment.GetEnvironmentVariable("rootFolder");

//        public Register(IRegisterToQueue dispatcherToQueue)
//        {
//            _dispatcherToQueue = dispatcherToQueue;
//        }

//        [FunctionName("Register")]
//        public void Run([QueueTrigger("moo-register-queue", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
//        {
//            try
//            {
//                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
//                bool queueSent = Process(myQueueItem, _queueName, _connectionString, _companyID);
//                if (queueSent)
//                {
//                    JToken parsedJson = JToken.Parse(myQueueItem);
//                    string formattedJson = parsedJson.ToString(Formatting.Indented);
//                    string filename = Path.Combine(_rootFolder, "cyware-register.json");
//                    if (!sftpService.CreateFile(filename, formattedJson))
//                    {
//                        log.LogError($"Error creating text file on SFTP server");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.LogError(ex.Message);
//                throw new Exception(ex.Message);
//            }
//        }

//        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
//        {
//            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
//        }
//    }
//}
