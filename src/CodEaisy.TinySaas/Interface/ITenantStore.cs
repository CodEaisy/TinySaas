using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantStore<T> where T : ITenant
    {
        ValueTask<T> GetTenant(string identifier);
        ValueTask<IEnumerable<T>> GetAllTenants();
    }
}
