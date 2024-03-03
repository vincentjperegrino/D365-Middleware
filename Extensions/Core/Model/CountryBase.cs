using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class CountryBase
    {
            
        public string country_id { get; init; }
        public string two_letter_abbreviation { get; init; }
        public string three_letter_abbreviation { get; init; }
        public string full_name_locale { get; init; }
        public string full_name_english { get; init; }


    }
}
