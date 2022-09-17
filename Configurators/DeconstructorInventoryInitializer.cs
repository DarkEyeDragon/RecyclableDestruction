using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bindito.Core;
using Timberborn.Buildings;
using Timberborn.Goods;
using Timberborn.GoodStackSystem;
using Timberborn.InventorySystem;
using Timberborn.Persistence;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

internal class DeconstructorInventoryInitializer : IDedicatedDecoratorInitializer<Inventory, Building>
{
    
    private static readonly string InventoryComponentName = "DeconstructionInventory";
    private readonly IGoodService _goodService;

    public DeconstructorInventoryInitializer(IGoodService goodService)
    {
        _goodService = goodService;
    }

    public void Initialize(Inventory decorator, Building subject)
    {
        DeconstructionInventory inventory = subject.GetComponent<DeconstructionInventory>();
        //var building = subject.GetComponentInParent<Building>();
        InventoryInitializer inventoryInitializer = new InventoryInitializer(_goodService, decorator, 100, InventoryComponentName);
        RecyclableDestruction.LOGGER.LogInfo("Inventory decorator:" + decorator);
        RecyclableDestruction.LOGGER.LogInfo("Inventory subject:" + inventory);
        AllowGoodsAsTakeable(inventoryInitializer, subject);
        AddItems(decorator, subject.BuildingCost);
        inventoryInitializer.HasPublicOutput();
        inventoryInitializer.Initialize();
        //decorator.Disable();
        inventory.InitializeInventory(decorator);
    }
    
    private void AllowGoodsAsTakeable(
        InventoryInitializer inventoryInitializer,
        Building building)
    {
        foreach (GoodAmountSpecification cost in building.BuildingCost)
        {
            StorableGoodAmount good = new StorableGoodAmount(StorableGood.CreateAsTakeable(cost.GoodId), cost.Amount);
            inventoryInitializer.AddAllowedGood(good);
        }
    }

    private void AddItems(Inventory inventory, IEnumerable<GoodAmountSpecification> cost)
    {
        foreach (var goodAmountSpecification in cost)
        {
            inventory.Give(goodAmountSpecification.ToGoodAmount());
        }
    }
    private int CalculateTotalCapacity(
        DeconstructionInventory simpleOutputInventory,
        IAllowedGoodProvider provider)
    {
        return provider == null ? simpleOutputInventory.Capacity : simpleOutputInventory.Capacity * provider.GetAllowedGoods().Count();
    }

}