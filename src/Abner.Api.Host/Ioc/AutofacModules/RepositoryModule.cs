using Abner.Domain.BlogAggregate;
using Abner.Domain.Core;
using Abner.EntityFrameworkCore.Contexts;
using Abner.EntityFrameworkCore.Repositories;
using Abner.Infrastructure.Core;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Abner.Api.Host;

internal class RepositoryModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Abner.Api.Host/"))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var conStr = configuration.GetConnectionString("Default");
        
        var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
        optionsBuilder.UseMySql(conStr, ServerVersion.AutoDetect(conStr));

        builder.RegisterType<BlogContext>()
            .WithParameter("options", optionsBuilder.Options)
            .AsSelf()
            .InstancePerLifetimeScope()
            .PropertiesAutowired(new AbnerPropertySelector());

        builder
            .RegisterAssemblyTypes(typeof(BlogRepository).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();


        // 获取所有控制器类型并使用属性注入
        var controllerBaseType = typeof(ControllerBase);
        builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            .Where(t => t.IsAssignableTo(controllerBaseType) && t != controllerBaseType)
            .PropertiesAutowired();
    }
}
