using System;
using Microsoft.Xrm.Sdk;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using CRM_Plugin.Promo.Interface;
using Core_EntityHelper = CRM_Plugin.Core.Helper.EntityHelper;
using Helper = CRM_Plugin.Core.Helper.HashingAndEncryption;

namespace CRM_Plugin.Promo.Domain
{
    internal class PromoDefault : IPromo
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public PromoDefault(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public bool Process(Entity _entity)
        {

            return false;
        }

    }
}
