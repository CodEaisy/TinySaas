using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantService<T> where T: ITenant
    {
        Task<T> GetTenant();
    }
}
