using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;

namespace TestMagento.Domain
{
    public class Category : MagentoBase
    {
        [Fact]
        public void GetCategory_Working()
        {

       
            KTI.Moo.Extensions.Magento.Domain.Category MagentoCategory = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoCategory.Get();

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Category>>(response);
        }

        [Fact]
        public void GetCategoryByID_Working()
        {
            int CategoryID = 12;

            KTI.Moo.Extensions.Magento.Domain.Category MagentoCategory = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoCategory.Get(CategoryID);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Category>(response);
        }




        [Fact]
        public void AddCategory_Working()
        {

            KTI.Moo.Extensions.Magento.Domain.Category MagentoCategory = new(defaultURL, redisConnectionString, username, password);

            KTI.Moo.Extensions.Magento.Model.Category CategoryModel = new();

            //crmProduct.parentproductid
            CategoryModel.parent_id = 18;
            //crmProduct.name
            CategoryModel.Name = "Vodka";
            //crmProduct.statecode
            CategoryModel.is_active = true;
            //crmProduct.kti_mooenabled
            CategoryModel.include_in_menu = true;
            //crmProduct.kti_mossequence
            CategoryModel.position = 10;
            //CategoryModel.level = 3;
            CategoryModel.product_count = 0;


            var response = MagentoCategory.Add(CategoryModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Category>(response);
        }



        [Fact]
        public void UpdateCategory_Working()
        {

            KTI.Moo.Extensions.Magento.Domain.Category MagentoCategory = new(defaultURL, redisConnectionString, username, password);

            KTI.Moo.Extensions.Magento.Model.Category CategoryModel = new();

            CategoryModel.CategoryId = 16;
            CategoryModel.parent_id = 3;
            CategoryModel.Name = "KTI Special Edition 39";
            CategoryModel.is_active = true;
            CategoryModel.include_in_menu = true;
            CategoryModel.position = 10;
            CategoryModel.level = 3;
            CategoryModel.product_count = 0;


            var response = MagentoCategory.Update(CategoryModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Category>(response);
        }

        [Fact]
        public void DeleteCategory_Working()
        {

            KTI.Moo.Extensions.Magento.Domain.Category MagentoCategory = new(defaultURL, redisConnectionString, username, password);

            int CategoryId = 18;

            var response = MagentoCategory.Delete(CategoryId);

            Assert.True(response);
        }





    }
}
