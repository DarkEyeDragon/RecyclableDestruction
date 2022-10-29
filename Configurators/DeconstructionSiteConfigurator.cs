using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

[Configurator(SceneEntrypoint.InGame)]
public class DeconstructionSiteConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<DeconstructionSiteInventoryInitializer>().AsSingleton();
        containerDefinition.MultiBind<TemplateModule>().ToProvider<DeconstructionSiteTemplateModuleProvider>().AsSingleton();
    }
}