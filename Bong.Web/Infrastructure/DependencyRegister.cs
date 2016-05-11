using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;

using Bong.Core.Helper;
using Bong.Core.Configuration;
using Bong.Core.Infrastructure;
using Bong.Core.Infrastructure.Dependency;
using Bong.Core.Data;
using Bong.Data;
using Bong.Services.Customers;
using Bong.Services.Goods;
using Bong.Services.Media;
using Bong.Services.Orders;
using Bong.Services.Setting;
using Bong.Services.Install;
using Bong.Services.Authenticate;

namespace Bong.Web.Infrastructure
{
    public class DependencyRegister : IDependencyRegister 
    {
        public virtual void Register(ContainerBuilder builder)
        {
            // Http abstractions
            builder.RegisterModule<AutofacWebTypesModule>();

            // Helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            // Controller
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Data layer
            builder.Register<IDbContext>(c => new BongDbContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EFRepositoty<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // Services
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
            builder.RegisterType<SubscribeService>().As<ISubscribeService>().InstancePerLifetimeScope();
            builder.RegisterType<ViewedProductService>().As<IViewedProductService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<InstallService>().As<IInstallService>().InstancePerLifetimeScope();
            builder.RegisterType<FormAuthenticateService>().As<IAuthenticateService>().InstancePerLifetimeScope();

            // Setting
            builder.Register<SystemSetting>((c, p) =>
            {
                return c.Resolve<ISettingService>().LoadSetting<SystemSetting>();
            });

            // User Context
            builder.RegisterType<UserContext>().As<IUserContext>().InstancePerLifetimeScope();
        }
    }
}