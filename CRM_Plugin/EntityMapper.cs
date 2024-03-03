using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;

namespace CRM_Plugin
{
    public class EntityMapper
    {
        Dictionary<string, Dictionary<string, Type>> fields = new Dictionary<string, Dictionary<string, Type>>();

        /// <summary>
        /// Retrieves the entity fields metadata
        /// </summary>
        /// <param name="entity">entity name</param>
        public void LoadEntity(string entity, IOrganizationService service)
        {
            fields = new Dictionary<string, Dictionary<string, Type>>();
            var request = new Microsoft.Xrm.Sdk.Messages.RetrieveEntityRequest();
            request.RetrieveAsIfPublished = true;
            request.EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.All;
            request.LogicalName = entity;

            var result = (RetrieveEntityResponse)service.Execute(request);

            this.fields.Add(entity, result.EntityMetadata.Attributes.ToDictionary(x => x.LogicalName, x => GetConversionType(x.AttributeType)));
        }

        /// <summary>
        /// Return the type of field based on the CRM's AttributeTypeCode
        /// </summary>
        /// <param name="typecode">AttributeTypeCode</param>
        public Type GetConversionType(AttributeTypeCode? typecode)
        {
            switch (typecode.Value)
            {
                case AttributeTypeCode.BigInt: return typeof(Int64);
                case AttributeTypeCode.Boolean: return typeof(Boolean);
                case AttributeTypeCode.DateTime: return typeof(DateTime);
                case AttributeTypeCode.Decimal: return typeof(Decimal);
                case AttributeTypeCode.Double: return typeof(Double);
                case AttributeTypeCode.Integer: return typeof(Int32);
                case AttributeTypeCode.Memo: return typeof(String);
                case AttributeTypeCode.Uniqueidentifier: return typeof(Guid);
                default: return typeof(String);
            }
        }
        public Entity pushValueToEntityField(KeyValuePair<string, JToken> property, Entity entity)
        {
            try
            {
                var value = property.Value;
                var key = property.Key;

                if (value.Type == JTokenType.Object)
                {
                    if (value["lookup"] != null && value["lookup"].Type != JTokenType.Null)
                    {
                        if (value["Id"].Type != JTokenType.Null)
                            entity[key] = new EntityReference(value["lookup"].ToString(), value["Id"].ToObject<Guid>());
                        else
                            entity[key] = null;
                    }
                    else if (value["Value"] != null && value["Value"].Type != JTokenType.Null)
                        entity[key] = new OptionSetValue(value["Value"].ToObject<int>());
                    else if (value["Money"] != null && value["Money"].Type != JTokenType.Null)
                        entity[key] = new Money(value["Money"].Value<decimal>());
                    //else
                    //    entity[key] = Convert.ChangeType(value.Value<object>(), servicebus.cache.GetTypeForEntity(entity.LogicalName, key));
                }
                //else
                //    entity[key] = Convert.ChangeType(value.Value<object>(), servicebus.cache.GetTypeForEntity(entity.LogicalName, key));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format($"pushValueToEntityField failure : {ex.Message}."));
            }

            return entity;
        }

        /// <summary>
        /// Return the field's type for an attribute linked to an opportunity
        /// </summary>
        /// <param name="entity">Entity name</param>
        /// <param name="attributeName">Attribute name</param>
        public Type GetTypeForEntity(string entity, string attributeName, IOrganizationService service)
        {
            if (!this.fields.ContainsKey(entity))
                this.LoadEntity(entity, service);

            if (this.fields.ContainsKey(entity) && this.fields[entity].ContainsKey(attributeName))
            {
                if (this.fields[entity][attributeName] == null)
                    throw new Exception(String.Format("Type of CRM field {0}.{1} is null", entity, attributeName));
                else
                    return this.fields[entity][attributeName];
            }
            else
                throw new Exception(String.Format("Can't find field type in CRM for entity and field {0}.{1}", entity, attributeName));
        }
    }
}
