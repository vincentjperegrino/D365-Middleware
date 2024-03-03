using Azure.Storage.Blobs.Models;
using KTI.Moo.Extensions.Core.Service;
using Moo.FO.App.Queue.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moo.FO.App.Queue.Blob
{
    public class StoreTransactions
    {
        private readonly IBlobService _blobService;
        private bool _getAll = false;

        public StoreTransactions(IBlobService blobService, bool getAll = false)
        {
            _blobService = blobService;
            _getAll = getAll;
        }

        public Model.StoreTransactions GetPOSTransactions(string containerName, string rootFolder, string dateFilter)
        {
            List<string> fileNames = _getAll
                ? _blobService.GetFiles(containerName, rootFolder)
                : _blobService.GetFiles(containerName, rootFolder, dateFilter);

            return new Model.StoreTransactions
            {
                Headers = Get<StoreTransactionsHeader>(fileNames, "Headers", containerName),
                Lines = Get<StoreTransactionsLine>(fileNames, "Lines", containerName),
                Discounts = Get<StoreTransactionsDiscount>(fileNames, "Discounts", containerName),
                Payments = Get<StoreTransactionsPayment>(fileNames, "Tenders", containerName)
            };
        }

        public List<T> Get<T>(List<string> fileNames, string searchKeyword, string containerName)
        {
            return fileNames
                .Where(fileName => fileName.Contains(searchKeyword))
                .SelectMany(fileName =>
                {
                    string jsonContent = _blobService.ReadFile(containerName, fileName);

                    return JsonConvert.DeserializeObject<List<T>>(jsonContent);
                })
                .ToList();
        }
    }
}
