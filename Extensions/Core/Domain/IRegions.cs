using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain
{
    internal interface IRegions<T> where T : RegionBase
    {
        List<T> GetRegion(string RegionID);

    }
}
