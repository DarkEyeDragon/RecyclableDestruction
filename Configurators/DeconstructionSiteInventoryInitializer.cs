using System.Collections.Generic;
using RecyclableDestruction.DeconstructionSites;
using Timberborn.Buildings;
using Timberborn.ConstructionSites;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

public class DeconstructionSiteInventoryInitializer : IDedicatedDecoratorInitializer<DeconstructionInventory, DeconstructionSite>
{
    public void Initialize(DeconstructionInventory subject, DeconstructionSite decorator)
    {
        decorator.InitializeInventory(subject);
        RecyclableDestruction.LOGGER.LogInfo("InitializeInventory of DeconstructionSite");
    }
}