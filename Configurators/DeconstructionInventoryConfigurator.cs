using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

[Configurator(SceneEntrypoint.InGame)]
public class DeconstructionInventoryConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<DeconstructorInventoryInitializer>().AsSingleton();
        containerDefinition.MultiBind<TemplateModule>().ToProvider<DeconstructionInventoryTemplateModuleProvider>().AsSingleton();
    }
}