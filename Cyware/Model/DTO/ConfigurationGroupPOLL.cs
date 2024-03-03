using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class ConfigurationGroupPOLL
    {
        [SortOrder(1)]
        [MaxLength(10)]
        public  string CONFIGURATIONGROUP { get; set; }

        [SortOrder(2)]
        [MaxLength(60)]
        public  string NAME { get; set; }


        PollMapping helper = new PollMapping();

        public ConfigurationGroupPOLL(ConnfigurationGroup configurationGroup)
        {
            this.CONFIGURATIONGROUP = helper.FormatStringAddSpacePadding(configurationGroup.CONFIGURATIONGROUP, (typeof(ConfigurationGroupPOLL).GetProperty("CONFIGURATIONGROUP").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.NAME = helper.FormatStringAddSpacePadding(configurationGroup.NAME, (typeof(ConfigurationGroupPOLL).GetProperty("NAME").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(ConfigurationGroupPOLL configPOLL)
        {
            return helper.ConcatenateValues(configPOLL);
        }
    }
}
