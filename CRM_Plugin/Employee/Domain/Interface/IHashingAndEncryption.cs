

namespace CRM_Plugin.Domain
{
    public interface IEmployee
    {
        bool HashPassword(Models.Employee employee);
    }
}
