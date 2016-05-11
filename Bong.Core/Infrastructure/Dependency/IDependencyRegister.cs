using Autofac;

namespace Bong.Core.Infrastructure.Dependency
{
    public interface IDependencyRegister
    {
        void Register(ContainerBuilder builder);
    }
}
