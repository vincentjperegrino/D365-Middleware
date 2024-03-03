using System.Collections.Generic;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class Customer : Core.Model.CustomerBase
    {


        /// Order has no Email for Customer in Lazada

        //public string laz_city { get; set; }
        //public string laz_address1 { get; set; }
        //public string laz_address2 { get; set; }



        public string laz_emailaddress { get; set; }

        public List<Address> address { get; set; }

    }
}
