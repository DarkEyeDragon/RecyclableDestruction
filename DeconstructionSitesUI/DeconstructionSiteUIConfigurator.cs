using System;
using Bindito.Core;
using Timberborn.ConstructionSitesUI;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.DeconstructionSitesUI;

public class DeconstructionSiteUIConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        /*containerDefinition.Bind<DeconstructionSiteFragment>().AsSingleton();
        //containerDefinition.Bind<DeconstructionSiteFragmentInventory>().AsSingleton();
        containerDefinition.Bind<DeconstructionSitePanelDescriptionUpdater>().AsSingleton();
        containerDefinition.Bind<DeconstructionSitePriorityBatchControlRowItemFactory>().AsSingleton();
        containerDefinition.MultiBind<TemplateModule>().ToProvider(new Func<TemplateModule>(DeconstructionSitesUIConfigurator.ProvideTemplateModule)).AsSingleton();
        containerDefinition.MultiBind<EntityPanelModule>().ToProvider<DeconstructionSitesUIConfigurator.EntityPanelModuleProvider>().AsSingleton();*/
    }
}