using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KTI.Moo.Extensions.Core.Model
{
    public class CarrierBase
    {

        public virtual bool active { get; set; }
        public virtual string callback_url { get; set; }
        public virtual string callback_method { get; set; }
        public virtual long id { get; set; }
        public virtual string format { get; set; }
        public virtual string name { get; set; }
        public virtual bool service_discovery { get; set; }
        public virtual string admin_graphql_api_id { get; set; }


    }
}
