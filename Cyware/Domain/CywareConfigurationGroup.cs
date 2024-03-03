using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class CywareConfigurationGroup : IConfigurationGroup<KTI.Moo.Extensions.Cyware.Model.ConnfigurationGroup>
    {

        private ISFTPService _sftpService { get; init; }

        public CywareConfigurationGroup(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public ConnfigurationGroup Add(ConnfigurationGroup configGroupDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int configGroupID)
        {
            throw new NotImplementedException();
        }

        public ConnfigurationGroup Get(int configGroupID)
        {
            throw new NotImplementedException();
        }

        public ConnfigurationGroup Get(string configGroupID)
        {
            throw new NotImplementedException();
        }

        public bool Update(ConnfigurationGroup configGroupDetails)
        {
            throw new NotImplementedException();
        }

        public ConnfigurationGroup Upsert(ConnfigurationGroup configGroupDetails)
        {
            try
            {
                //Validate(bomDetails);
                var bomHeaderPOLL = new ConfigurationGroupPOLL(configGroupDetails);
                string FormattedString = bomHeaderPOLL.Concat(bomHeaderPOLL);


                string filename = "POLL102.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.ConnfigurationGroup();
                }
                return new Model.ConnfigurationGroup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
