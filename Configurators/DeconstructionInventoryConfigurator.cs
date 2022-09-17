using Bindito.Core;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

public class DeconstructionInventoryConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<DeconstructorInventoryInitializer>().AsSingleton();
        containerDefinition.MultiBind<TemplateModule>().ToProvider<DeconstructionInventoryTemplateModuleProvider>().AsSingleton();
    }
}