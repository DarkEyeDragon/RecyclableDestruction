using System.Collections.Generic;
using System.Linq;
using Timberborn.Buildings;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

internal class DeconstructorInventoryInitializer : IDedicatedDecoratorInitializer<DeconstructionInventory, Inventory>
{
    private static readonly string InventoryComponentName = "DeconstructionInventory";
    private readonly IGoodService _goodService;

    public DeconstructorInventoryInitializer(IGoodService goodService)
    {
        _goodService = goodService;
    }

    public void Initialize(DeconstructionInventory decorator, Inventory subject)
    {
        if (decorator == null)
        {
            RecyclableDestruction.LOGGER.LogInfo("Decorator is NULL");
        }

        var building = subject.GetComponent<Building>();
        var buildingCost = building.BuildingCost;
        var hasCost = buildingCost is { Count: > 0 };
        if (hasCost)
        {
            RecyclableDestruction.LOGGER.LogInfo($"Initialize with non empty inv: {building.name}");
            var goods = new List<StorableGoodAmount>();
            foreach (var cost in buildingCost)
            {
                var storableGood = StorableGood.CreateAsTakeable(cost.GoodId);
                goods.Add(new StorableGoodAmount(storableGood, cost.Amount));
            }

            var inventoryInitializer = new InventoryInitializer(_goodService, subject, goods.Sum(good => good.Amount),
                InventoryComponentName);

            inventoryInitializer.AddAllowedGoods(goods);
            inventoryInitializer.HasPublicOutput();
            inventoryInitializer.Initialize();

            AddItems(subject, goods);
            decorator.InitializeInventory(subject);
            RecyclableDestruction.LOGGER.LogInfo("Initialized");
        }
        else
        {
            RecyclableDestruction.LOGGER.LogInfo($"Initialize with empty inv: {building.name}");
            var inventoryInitializer = new InventoryInitializer(_goodService, subject, 0, InventoryComponentName);
            inventoryInitializer.Initialize();
            RecyclableDestruction.LOGGER.LogInfo("Initialized");
        }

        decorator.InitializeInventory(subject);
        //decorator.Disable();
    }

    private void AddItems(Inventory inventory, IEnumerable<StorableGoodAmount> goods)
    {
        foreach (var good in goods) inventory.Give(new GoodAmount(good.StorableGood.GoodId, good.Amount));
    }
}