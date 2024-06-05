using Autofac;

namespace BusinessLogicLayer.Autofac
{
    public class BusinessLogicLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BusinessLogicLayerModule).Assembly)
                .Where(t => t.Name.Contains("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyOpenGenericTypes(typeof(BusinessLogicLayerModule).Assembly)
                .Where(t => t.Name.Contains("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyOpenGenericTypes(typeof(BusinessLogicLayerModule).Assembly)
                .Where(t => t.Name.Contains("Handler"))
                .AsImplementedInterfaces();
        }
    }
}
