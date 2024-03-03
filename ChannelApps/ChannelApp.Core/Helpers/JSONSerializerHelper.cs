namespace KTI.Moo.ChannelApps.Core.Helpers;

public class JSONSerializer
{
    public class OriginalNameContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Let the base class create all the JsonProperties 
            // using the short names
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            // Now inspect each property and replace the 
            // short name with the real property name
            foreach (JsonProperty prop in list)
            {
            
                prop.PropertyName = prop.UnderlyingName;
            }

            return list;
        }
    }

    public class DontIgnoreResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
     
            // Let the base class create all the JsonProperties 
            // using the short names
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            // Now inspect each property and replace the 
            // short name with the real property name
            foreach (JsonProperty prop in list)
            {
            
                if (prop.PropertyName == "companyid")
                {         
                    prop.Ignored = false;
                }
            }

            return list;
        }
    }

    public static readonly DefaultContractResolver IgnoreIsSpecifiedMembersResolver =
    new DefaultContractResolver { IgnoreIsSpecifiedMembers = false
    
    };




}
