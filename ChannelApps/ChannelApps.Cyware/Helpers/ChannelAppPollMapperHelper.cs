using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChannelApps.Cyware.Helpers
{
    public class ChannelAppPollMapperHelper
    {
        public static string ConcatenateValues<T>(IEnumerable<T> items)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var orderedProperties = typeof(T)
                .GetProperties()
                .OrderBy(prop => prop.GetCustomAttribute<SortOrderAttribute>()?.Order ?? int.MaxValue);

            var concatenatedValues = items
                .Select(item => orderedProperties.Select(property => property.GetValue(item)))
                .SelectMany(values => values)
                .Select(value => value.ToString());

            stringBuilder.Append(string.Join("", concatenatedValues));

            return stringBuilder.ToString() + "<EOF>";
        }
    }
}
