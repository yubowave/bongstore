using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace Bong.Core.Infrastructure.Dependency
{
    public class IoContainer
    {
        private readonly IContainer _container;

        public IoContainer(IContainer container)
        {
            _container = container;
        }

        public IContainer Container 
        { 
            get
            {
                return _container;
            }
        }

        public ILifetimeScope GetScope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        public object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null) scope = GetScope();

            return scope.Resolve(type);
        }

        public T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)  scope = GetScope();

            if(string.IsNullOrEmpty(key))
                return scope.Resolve<T>();

            return scope.ResolveKeyed<T>(key);
        }

        public T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null) scope = GetScope();

            if (string.IsNullOrEmpty(key))
                return scope.Resolve<IEnumerable<T>>().ToArray();

            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }
    }
}