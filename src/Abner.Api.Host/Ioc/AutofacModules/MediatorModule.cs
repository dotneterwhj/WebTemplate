using Abner.Application.Core;
using Abner.Infrastructure.Core;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using System.Collections.Concurrent;
using System.Reflection;
using Abner.Application.Behaviors;
using Abner.Application.BlogApp;

namespace Abner.Api.Host;

internal class MediatorModule : Autofac.Module
{
    //protected override void Load(ContainerBuilder builder)
    //{
    //    builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
    //        .AsImplementedInterfaces();

    //    // Register all the CommandHandler classes (they implement ICommandHandler) in assembly holding the Commands
    //    builder.RegisterAssemblyTypes(typeof(CommandHandler<,>).GetTypeInfo().Assembly)
    //        .AsClosedTypesOf(typeof(ICommandHandler<,>));

    //    // Register the DomainEventHandler classes (they implement IDomainEventHandler<>) in assembly holding the Domain Events
    //    builder.RegisterAssemblyTypes(typeof(DomainEventHandler<>).GetTypeInfo().Assembly)
    //        .AsClosedTypesOf(typeof(IDomainEventHandler<>));

    //    // Register the Command's Validators (Validators based on FluentValidation library)
    //    builder
    //        .RegisterAssemblyTypes(typeof(CommandValidator<>).GetTypeInfo().Assembly)
    //        .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
    //        .AsImplementedInterfaces();


    //    builder.Register<ServiceFactory>(context =>
    //    {
    //        var componentContext = context.Resolve<IComponentContext>();
    //        return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
    //    });

    //    builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    //    builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    //    builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    //}

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterSource(new ScopedContravariantRegistrationSource(
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)
        ));

        //builder
        //        .RegisterType<Mediator>()
        //        .As<IMediator>()
        //        .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        var mediatrOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>),
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
                .RegisterAssemblyTypes(ThisAssembly, typeof(BlogQuery).Assembly)
                .AsClosedTypesOf(mediatrOpenType)
                .FindConstructorsWith(new AllConstructorFinder())
                .AsImplementedInterfaces();
        }

        //builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        builder.Register<ServiceFactory>(ctx =>
        {
            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        // builder.RegisterGeneric(typeof(TransactionBehaviour<,,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(BlogContextTransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    }

    private class ScopedContravariantRegistrationSource : IRegistrationSource
    {
        private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
        private readonly List<Type> _types = new List<Type>();

        public ScopedContravariantRegistrationSource(params Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (!types.All(x => x.IsGenericTypeDefinition))
                throw new ArgumentException("Supplied types should be generic type definitions");
            _types.AddRange(types);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service,
            Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var components = _source.RegistrationsFor(service, registrationAccessor);
            foreach (var c in components)
            {
                var defs = c.Target.Services
                    .OfType<TypedService>()
                    .Select(x => x.ServiceType.GetGenericTypeDefinition());

                if (defs.Any(_types.Contains))
                    yield return c;
            }
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
    }

    internal class AllConstructorFinder : IConstructorFinder
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
            new ConcurrentDictionary<Type, ConstructorInfo[]>();


        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            var result = Cache.GetOrAdd(targetType,
                t => t.GetTypeInfo().DeclaredConstructors.ToArray());

            return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType);
        }
    }
}