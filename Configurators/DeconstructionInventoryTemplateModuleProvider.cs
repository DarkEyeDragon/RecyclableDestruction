using Bindito.Core;
using Timberborn.Buildings;
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
        builder.AddDecorator<Building, DeconstructionInventory>();
        builder.AddDedicatedDecorator(_deconstructorInventoryInitializer);
        return builder.Build();
    }
}