using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Model.DTO.Search;
using KTI.Moo.Extensions.Magento.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Helper
{
    public static class SearchHelper
    {
        public static Dictionary<string, string> AddSearchParameter(this Dictionary<string, string> currentParameters, List<Order> ListOrder, int pageSize, int currentPageNumber)
        {

            return currentParameters.AddSearchParameter(ListOrder).AddSearchPaging(pageSize, currentPageNumber);

        }

        public static Dictionary<string, string> AddSearchParameter(this Dictionary<string, string> currentParameters, List<Order> ListOrder)
        {
            string condition_type = "in";

            var OrderIdList = ListOrder.Select(order => order.kti_sourceid);
            var OrderIdListDelimetedbyComma = string.Join<string>(",", OrderIdList);


            var searchParameters = new List<SearchParameters>() {

                 new SearchParameters()
                 {
                     condition_type = condition_type,
                     field = "order_id",
                     filters = 0,
                     filter_groups = 0,
                     value = OrderIdListDelimetedbyComma
                 }

            };



            return currentParameters.AddSearchParameters(searchParameters);

        }


        public static Dictionary<string, string> AddSearchParameter(this Dictionary<string, string> currentParameters, List<Customer> ListCustomers, int pageSize, int currentPageNumber)
        {
            string condition_type = "eq";
            return currentParameters.AddSearchParameter(ListCustomers, pageSize, currentPageNumber, condition_type);

        }


        public static Dictionary<string, string> AddSearchParameterRange(this Dictionary<string, string> currentParameters, Customer FromListCustomers, Customer ToListCustomers, int pageSize, int currentPageNumber)
        {
            string condition_type = "range";
            List<Customer> ListCustomers = new()
            {
                FromListCustomers,
                ToListCustomers
            };

            return currentParameters.AddSearchParameter(ListCustomers, pageSize, currentPageNumber, condition_type);

        }


        public static Dictionary<string, string> AddSearchParameterEmail(this Dictionary<string, string> currentParameters, string email)
        {
            List<SearchParameters> searchParameters = new()
            {
                new SearchParameters()
                {
                    field = "email",
                    value = email,
                    condition_type = "eq",
                    filter_groups = 0,
                    filters = 0
                }

            };

            return currentParameters.AddSearchParameters(searchParameters);

        }



        public static Dictionary<string, string> AddSearchParameterRangeUpdatedDate(this Dictionary<string, string> currentParameters, DateTime datefrom, DateTime dateto, int pageSize, int currentPageNumber)
        {
            List<SearchParameters> searchParameters = new()
            {
                new SearchParameters()
                {
                    field = "updated_at",
                    value = datefrom.ToString("yyyy-MM-dd HH:mm:ss"),
                    condition_type = "from",
                    filter_groups = 0,
                    filters = 0

                },
                new SearchParameters()
                {
                    field = "updated_at",
                    value = dateto.ToString("yyyy-MM-dd HH:mm:ss"),
                    condition_type = "to",
                    filter_groups = 1,
                    filters = 0

                }


            };

            return currentParameters.AddSearchParameters(searchParameters).AddSearchPaging(pageSize, currentPageNumber);

        }

        public static Dictionary<string, string> AddSearchParameterRangeCreatedDate(this Dictionary<string, string> currentParameters, DateTime datefrom, DateTime dateto, int pageSize, int currentPageNumber)
        {
            List<SearchParameters> searchParameters = new()
            {
                new SearchParameters()
                {
                    field = "created_at",
                    value = datefrom.ToString("yyyy-MM-dd HH:mm:ss"),
                    condition_type = "from",
                    filter_groups = 0,
                    filters = 0

                },
                new SearchParameters()
                {
                    field = "created_at",
                    value = dateto.ToString("yyyy-MM-dd HH:mm:ss"),
                    condition_type = "to",
                    filter_groups = 1,
                    filters = 0

                }


            };

            return currentParameters.AddSearchParameters(searchParameters).AddSearchPaging(pageSize, currentPageNumber);

        }



        private static Dictionary<string, string> AddSearchParameter(this Dictionary<string, string> currentParameters, List<Customer> ListCustomers, int pageSize, int currentPageNumber, string conditon_type)
        {
            int filter_groups = 0;
            int filters = 0;

            string conditontype = conditon_type;

            if (conditon_type == "range")
            {
                conditontype = "from";

            }

            List<SearchParameters> searchParameters = new();

            foreach (var customer in ListCustomers)
            {
                AvailableForSearching _AvailableForSearching = new(customer);

                foreach (var Searchproperties in _AvailableForSearching.GetType().GetProperties())
                {
                    string PropertyValue = "";

                    try
                    {
                        PropertyValue = Searchproperties.GetValue(_AvailableForSearching, (object[])null).ToString();
                    }
                    catch (System.Exception ex)
                    {
                        continue;
                    }


                    string IntType = "Int32";
                    string StringType = "String";
                    string DateTimeType = "DateTime";

                    string IntValueZero = "0";


                    if (Searchproperties.PropertyType.Name == IntType && (string.IsNullOrWhiteSpace(PropertyValue) || PropertyValue == IntValueZero))
                    {
                        continue;
                    }

                    if (Searchproperties.PropertyType.Name == StringType && string.IsNullOrWhiteSpace(PropertyValue))
                    {
                        continue;
                    }

                    if (Searchproperties.PropertyType.Name == DateTimeType)
                    {
                        DateTime oDate = Convert.ToDateTime(PropertyValue);
                        int minyearallowed = 2000;

                        if (oDate.Year < minyearallowed)
                        {
                            continue;
                        }

                    }


                    searchParameters.Add(new SearchParameters()
                    {
                        filter_groups = filter_groups,
                        filters = filters,
                        field = Searchproperties.Name,
                        value = PropertyValue,
                        condition_type = conditontype
                    });


                    filters++;


                    if (conditon_type == "range")
                    {

                        filter_groups++;
                        conditontype = "to";

                    }
                }


            }

            return currentParameters.AddSearchParameters(searchParameters).AddSearchPaging(pageSize, currentPageNumber);

        }


        public static Dictionary<string, string> AddSearchParameter(this Dictionary<string, string> currentParameters, string field, string value, string condition_type)
        {
            //since 1 parameter is needed
            int filter_groups = 0;
            int filters = 0;

            List<SearchParameters> searchParameters = new()
            {
                new SearchParameters()
                {
                    filter_groups = filter_groups,
                    filters = filters,
                    field = field,
                    value = value,
                    condition_type = condition_type
                }

            };

            return currentParameters.AddSearchParameters(searchParameters);

        }


        public static Dictionary<string, string> AddSearchParameters(this Dictionary<string, string> currentParameters, List<SearchParameters> searchParameters)
        {

            foreach (var param in searchParameters)
            {

                currentParameters.Add($"searchCriteria[filter_groups][{param.filter_groups}][filters][{param.filters}][field] ", param.field);
                currentParameters.Add($"searchCriteria[filter_groups][{param.filter_groups}][filters][{param.filters}][value] ", param.value);
                currentParameters.Add($"searchCriteria[filter_groups][{param.filter_groups}][filters][{param.filters}][condition_type] ", param.condition_type);
            }

            return currentParameters;

        }




        public static Dictionary<string, string> AddSearchPaging(this Dictionary<string, string> currentParameters, int pagesize, int currentPage)
        {

            currentParameters.Add($"searchCriteria[pageSize]", pagesize.ToString());
            currentParameters.Add($"searchCriteria[currentPage]", currentPage.ToString());

            return currentParameters;

        }





    }
}

