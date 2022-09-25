using Autofac;
using DotNetCore.CAP;
using System.Reflection;

namespace Abner.Api.Host;

internal class CapModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ICapPublisher).GetTypeInfo().Assembly)
        .AsImplementedInterfaces()
        .PropertiesAutowired();
    }
}
