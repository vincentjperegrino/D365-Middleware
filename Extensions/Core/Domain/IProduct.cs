namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IProduct<T> where T : Core.Model.ProductBase
    {
        // basic crud methods
        T Get(long productId);
        T Get(string productSku);
        // T Get(long productId, string productSku = null);
        T Add(T product);
        bool Update(T product);
        bool Delete(T product);

        // create if not exists, update otherwise
        T Upsert(T product);
        bool AddImage(long productId, string externalImageUrl, bool primary = false);
        bool RemoveImage(long productId, int index);
        bool SetPrimaryImage(long productId, int index);
        // bool AddVariantImage(long productId, string sku, string externalImageUrl, bool primary = false);
        // bool RemoveVariantImage(long productId, string sku, string imageUrl);
        // bool SetVariantPrimaryImage(long productId, string sku, string imageUrl);
    }
}