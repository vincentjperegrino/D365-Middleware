using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class ProductCategoryBase
    {
        public int CategoryHierarchy { get; set; }
        public int ChangeStatus { get; set; }
        public string Code { get; set; }
        public int DefaultProjectGlobalCategory { get; set; }
        public decimal DefaultThreshold_PSN { get; set; }
        public int InstanceRelationType { get; set; }
        public int IsActive { get; set; }
        public int IsCategoryAttributesInherited { get; set; }
        public int IsTangible { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int NestedSetLeft { get; set; }
        public int NestedSetRight { get; set; }
        public int ParentCategory { get; set; }
        public int PKWiUCode { get; set; }
        public string SharedCategory_CategoryId { get; set; }
        public string EcoResCategoryHierarchy_Name { get; set; }
        public int EcoResCategory1_CategoryHierarchy { get; set; }
        public string EcoResCategory1_Name { get; set; }
        public string EcoResCategoryHierarchy1_Name { get; set; }
        public int AxRecId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        //Customized
        //[CompanyIdAttribute]
        public int companyid { get; set; }
    }
}
