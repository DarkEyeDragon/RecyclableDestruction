using Bindito.Core;
using RecyclableDestruction.DeconstructionSites;
using RecyclableDestruction.Types;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using Timberborn.ConstructionSites;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

internal class DeconstructionInventoryTemplateModuleProvider : IProvider<TemplateModule>
{
    private readonly DeconstructorInventoryInitializer _deconstructorInventoryInitializer;

    public DeconstructionInventoryTemplateModuleProvider(
        DeconstructorInventoryInitializer deconstructorInventoryInitializer)
    {
        _deconstructorInventoryInitializer = deconstructorInventoryInitializer;
    }
    
    public TemplateModule Get()
    {
        TemplateModule.Builder builder = new TemplateModule.Builder();
        builder.AddDecorator<DeconstructionSite, DeconstructionInventory>();
        builder.AddDedicatedDecorator(_deconstructorInventoryInitializer);
        return builder.Build();
    }
}