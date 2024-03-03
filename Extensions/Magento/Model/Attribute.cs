using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;


namespace KTI.Moo.Extensions.Magento.Model
{
    public class Attribute
    {
        //private int _ValueInt;
        //private string _ValueString;
        //private string[] _ValueStringArray;
        //private double _ValueDouble;
        //private long _Valuelong;
        //private dynamic _values;
        //private string _types;

        [JsonProperty("attribute_code")]
        public string attribute_code { get; set; }

        [JsonProperty("value")]
        public dynamic value { get; set; }

        //[JsonProperty("value")]
        //public dynamic value
        //{
        //    set
        //    {

        //        if (value is string)
        //        {
        //            _types = "int";
        //            bool ifint = int.TryParse(value, out _ValueInt);
        //            if (ifint == false)
        //            {
        //                _types = "double";
        //                bool success = double.TryParse(value, out _ValueDouble);
        //                if (success == false)
        //                {
        //                    _types = "string";
        //                    _ValueString = value;
        //                }

        //            }

        //        }

        //        else if (value is double)
        //        {
        //            _types = "double";
        //            _ValueDouble = value;
        //        }

        //        else if (value is long)
        //        {
        //            _types = "long";
        //            _Valuelong = value;
        //        }

        //        else
        //        {
        //            _values = value;
        //        }


        //    }

        //    get
        //    {

        //        if (_types == "stringArray")
        //        {
        //            return _ValueStringArray;

        //        }

        //        if (_types == "string")
        //        {
        //            return _ValueString;

        //        }

        //        if (_types == "double")
        //        {
        //            return _ValueDouble;

        //        }

        //        if (_types == "int")
        //        {
        //            return _ValueInt;

        //        }

        //        if (_types == "long")
        //        {
        //            return _Valuelong;

        //        }

        //        return _values;

        //    }
        //}



        //[JsonIgnore]
        //public string ValueString
        //{
        //    get => _ValueString;

        //    set => this._values = value;
        //}


        //[JsonIgnore]
        //public double ValueDouble
        //{
        //    get => _ValueDouble;

        //    set => this._values = value;
        //}



        //[JsonIgnore]
        //public int ValueInt
        //{
        //    get => _ValueInt;

        //    set => this._values = value;
        //}


        //[JsonIgnore]
        //public string[] ValueStringArray
        //{
        //    get => _ValueStringArray;

        //    set => this._values = value;
        //}





    }
}
