using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain;

public interface IPromoPlatform<GenericPromo, GenericPromoProduct> where GenericPromo : Model.PromoBase where GenericPromoProduct : Model.PromoProductBase
{
    GenericPromo Add(GenericPromo promo);
    GenericPromo Update(GenericPromo promo);
    GenericPromo Upsert(GenericPromo promo);
    GenericPromo Get(string promocode);
    GenericPromo GetFromID(string promocodeid);
    bool Activate(string promocode);
    bool ActivateFromID(string promocodeid);
    bool Deactivate(string promocode);
    bool DeactivateFromID(string promocodeid);
    bool AddProducts(List<GenericPromoProduct> promo);
    bool DeleteProducts(List<GenericPromoProduct> promo);
    List<GenericPromoProduct> GetProductList(string promocode);
    List<GenericPromoProduct> GetProductListFromID(string promocodeid);
}
