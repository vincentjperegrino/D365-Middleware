using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class Store : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IStores<KTI.Moo.Extensions.Cyware.Model.Store>> _mockStore;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.Store _store;

        public Store()
        {
            _mockStore = new Mock<IStores<KTI.Moo.Extensions.Cyware.Model.Store>>();
            _queueServiceMock = new Mock<IQueueService>();
            _store = new KTI.Moo.Extensions.Cyware.App.Dispatcher.Store(_queueServiceMock.Object, _mockStore.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.Discount());
            var logger = new NullLogger<Price>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _store.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockStore.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.Store>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void StoreDomain_Upsert_ReturnsStoreDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.Stores _stores = new KTI.Moo.Extensions.Cyware.Domain.Stores(config);
            KTI.Moo.Extensions.Cyware.Model.Store storeModel = new KTI.Moo.Extensions.Cyware.Model.Store()
            {
                strNum = 1234,
                strNam = "StoreA",
                strAd1 = "Address Line 1",
                strAd2 = "Address Line 2",
                strAd3 = "Address Line 3",
                strPhn = "123-456-7890",
                stMngr = "ManagerA",
                strHdo = "Head Office",
                strCod = "StoreCode123",
                strTxc = "TaxCode123",
                strLan = "LanguageA"
            };

            //Act
            var result = _stores.Upsert(storeModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}
