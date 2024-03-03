using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class ProductCategory : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IProductCategory<KTI.Moo.Extensions.Cyware.Model.ProductCategory>> _mockProductCategory;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.ProductCategory _productCategory;

        public ProductCategory()
        {
            _mockProductCategory = new Mock<IProductCategory<KTI.Moo.Extensions.Cyware.Model.ProductCategory>>();
            _queueServiceMock = new Mock<IQueueService>();
            _productCategory = new KTI.Moo.Extensions.Cyware.App.Dispatcher.ProductCategory(_queueServiceMock.Object, _mockProductCategory.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.ProductCategory());
            var logger = new NullLogger<ProductCategory>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _productCategory.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockProductCategory.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.ProductCategory>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void ProductCategoryDomain_Upsert_ReturnsProductCategoryDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.ProductCategory _productCategory = new KTI.Moo.Extensions.Cyware.Domain.ProductCategory(config);
            KTI.Moo.Extensions.Cyware.Model.ProductCategory productCategoryModel = new KTI.Moo.Extensions.Cyware.Model.ProductCategory()
            {
                department = "DeptA",
                sub_dept = "SubDeptA",
                cy_class = "ClassA",
                sub_class = "SubClassA",
                name = "ObjectA",
                planned_gm = "0.15"
            };

            //Act
            var result = _productCategory.Upsert(productCategoryModel);

            //Assert
            Assert.NotNull(result);
        }
    }
}
