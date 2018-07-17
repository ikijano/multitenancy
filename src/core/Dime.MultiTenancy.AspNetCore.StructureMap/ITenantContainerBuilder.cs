using StructureMap;
using System.Threading.Tasks;

namespace Dime.Multitenancy.StructureMap
{
    public interface ITenantContainerBuilder<TTenant>
    {
        Task<IContainer> BuildAsync(TTenant tenant);
    }
}