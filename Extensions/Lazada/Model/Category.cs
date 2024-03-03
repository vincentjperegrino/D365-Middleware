using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using KTI.Moo.Extensions.Lazada.Utils;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class Category : Core.Model.CategoryBase
    {
        public List<Category> Children { get; set; } = null;
        public bool Var { get; set; }
        public bool IsLeaf { get; set; }

        public Category Find(string name, bool exactMatch = true)
        {
            return this.Descendants().Where(c => c.IsLeaf && exactMatch ? c.Name.Equals(name) : c.Name.ToLower().Contains(name.ToLower())).FirstOrDefault();
        }

        public Category Find(int id)
        {
            return this.Descendants().Where(c => c.IsLeaf && c.CategoryId == id).FirstOrDefault();
        }
    }

    public class Attribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string AttributeType { get; set; }
        public string InputType { get; set; }
        public bool IsKeyProp { get; set; }
        public bool IsSaleProp { get; set; }
        public bool IsMandatory { get; set; }
        public List<AttributeOption> Options { get; set; }
    }

    public class AttributeOption
    {
        public string Name { get; set; }
        public string EnName { get; set; }
        public int Id { get; set; }
    }

    public record CategorySuggestion
    {
        [JsonPropertyName("categoryId")]
        public int Id { get; set; }
        [JsonPropertyName("categoryName")]
        public string Name { get; set; }
        [JsonPropertyName("categoryPath")]
        public string Path { get; set; }
    }
}
