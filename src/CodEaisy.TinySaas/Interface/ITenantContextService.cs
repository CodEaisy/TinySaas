using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantContextService<T> where T: ITenant
    {
        Task<ITenantContext<T>> GetTenantContext();
    }
}
