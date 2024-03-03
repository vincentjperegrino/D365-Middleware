using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.Models
{
    public class ExtensionQueueMessageModel
    {
        public string FileName  { get; set; }
        public string ModuleType { get; set; }
        public int Count { get; set; }
        public string CompanyId { get; set; }

        public ExtensionQueueMessageModel(string fileName, string moduleType, int count, string companyId)
        {
            FileName = fileName;
            ModuleType = moduleType;
            Count = count;
            CompanyId = companyId;
        }
    }
}
