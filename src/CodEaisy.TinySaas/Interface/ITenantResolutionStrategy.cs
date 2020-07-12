using System.Threading.Tasks;

namespace CodEaisy.TinySaas.Interface
{
    public interface ITenantResolutionStrategy
    {
        ValueTask<string> GetTenantIdentifierAsync();
    }
}
