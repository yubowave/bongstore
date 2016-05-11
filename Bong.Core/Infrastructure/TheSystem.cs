using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Runtime.CompilerServices;

using Autofac;
using Autofac.Integration.Mvc;
using Bong.Core.Infrastructure.Dependency;
using Bong.Core.Configuration;

namespace Bong.Core.Infrastructure
{
    public class TheSystem : ISystem
    {
        #region Singleton
        private TheSystem() { }

        private static ISystem _sys;
        public static ISystem Current
        {
            get 
            {
                if (_sys == null) Init();
                return _sys; 
            }
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Init()
        {
            if (_sys == null)
            {
                var config = ConfigurationManager.GetSection("BongConfig") as BongConfig;
                _sys = new TheSystem();
                _sys.Initialize(config);
            }
        }

        #endregion

        #region Fields
        private IoContainer _container;
        #endregion

        #region Properties
        public IoContainer IoContainer 
        {
            get { return _container; }
        }
        #endregion

        #region Implementation of ISystem
        
        public void Initialize(BongConfig config)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            // dependencies
            builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<BongConfig>().SingleInstance();
            builder.RegisterInstance(this).As<ISystem>().SingleInstance();
            builder.Update(container);

            // set dependency resolver
            this._container = new IoContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public void UpdateContainer(IDependencyRegister register)
        {
            var builder = new ContainerBuilder();
            register.Register(builder);
            builder.Update(IoContainer.Container);
        }

        public T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }
        
        #endregion
    }
}
