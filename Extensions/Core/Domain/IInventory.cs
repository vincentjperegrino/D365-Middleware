using KTI.Moo.Extensions.Core.Model;

namespace KTI.Moo.Extensions.Core.Domain;

public interface IInventory<T> where T : InventoryBase
{
    T Get(int ProductID);
    T Get(string ProductSku);
 
    bool Update(T InventoryDetails);
    bool StockIn(T InventoryDetails);
    bool StockOut(T InventoryDetails);

}
