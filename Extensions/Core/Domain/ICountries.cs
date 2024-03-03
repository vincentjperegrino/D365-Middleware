using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface ICountries<T> where T : CountryBase
    {

        List<T> GetCountries();

        T GetCountry(string CountryID);

    }
}
