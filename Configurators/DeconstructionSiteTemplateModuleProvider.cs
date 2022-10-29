using Bindito.Core;
using RecyclableDestruction.DeconstructionSites;
using RecyclableDestruction.Types;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using Timberborn.ConstructionSites;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

public class DeconstructionSiteTemplateModuleProvider : IProvider<TemplateModule>
{
    private readonly DeconstructionSiteInventoryInitializer _deconstructionSiteInventoryInitializer;

    public DeconstructionSiteTemplateModuleProvider(DeconstructionSiteInventoryInitializer deconstructionSiteInventoryInitializer)
    {
        _deconstructionSiteInventoryInitializer = deconstructionSiteInventoryInitializer;
    }

    public TemplateModule Get()
    {
        TemplateModule.Builder builder = new TemplateModule.Builder();
        builder.AddDecorator<Building, Deconstructable>();
        builder.AddDecorator<Building, DeconstructionSite>();
        builder.AddDedicatedDecorator<DeconstructionSite, DeconstructionInventory>(_deconstructionSiteInventoryInitializer);
        RecyclableDestruction.LOGGER.LogInfo("Building <- Deconstructable");
        return builder.Build();
    }
}