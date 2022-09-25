using Autofac;

namespace Abner.Api.Host;

public class AutofacDependencyInjection
{
    public static void Init(ContainerBuilder builder)
    {
        builder.RegisterModule(new MediatorModule());
        //builder.RegisterModule(new CapModule());
        builder.RegisterModule(new RepositoryModule());
    }
}
