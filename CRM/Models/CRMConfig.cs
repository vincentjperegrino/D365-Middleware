using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class CRMConfig
    {
        public CRMConfig(CRMConfig _crmConfig)
        {
            #region properties
            this.auth_url = _crmConfig.auth_url;
            this.resource_url = _crmConfig.resource_url;
            this.client_id = _crmConfig.client_id;
            this.client_secret = _crmConfig.client_secret;
            this.crmbase_url = _crmConfig.crmbase_url;
            this.unitgroup_path = _crmConfig.unitgroup_path;
            #endregion
        }

        public CRMConfig()
        {
        }

        #region Properties
        public string auth_url { get; set; }
        public string resource_url { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string crmbase_url { get; set; }
        public string unitgroup_path { get; set; }
        #endregion
    }
}
