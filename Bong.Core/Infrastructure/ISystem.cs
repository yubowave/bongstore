using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bong.Core.Infrastructure.Dependency;
using Bong.Core.Configuration;

namespace Bong.Core.Infrastructure
{
    public interface ISystem
    {
        IoContainer IoContainer { get; }

        void Initialize(BongConfig config);

        void UpdateContainer(IDependencyRegister register);

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
