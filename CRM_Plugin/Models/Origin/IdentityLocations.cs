#region Namespaces
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Origin
{
    public class IdentityLocations
    {
        public IdentityLocations(IdentityLocations identityLocations)
        {
            #region properties
            #endregion
        }

        #region Properties
        [JsonProperty(PropertyName = "Id")]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "CompanyId")]
        public int CompanyId { get; set; }
        [JsonProperty(PropertyName = "ParentLocationId")]
        public int ParentLocationId { get; set; }
        [DataType(DataType.Text)]
        [StringLength(50)]
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public int Status { get; set; }
        [StringLength(255)]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "ContactNumber")]
        public string ContactNumber { get; set; }
        [JsonProperty(PropertyName = "OIC")]
        public int OIC { get; set; }
        #endregion
    }
}
