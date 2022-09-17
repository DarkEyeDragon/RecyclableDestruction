using System.Collections.Generic;
using System.Linq;
using Timberborn.Buildings;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace RecyclableDestruction.Configurators;

internal class DeconstructorInventoryInitializer : IDedicatedDecoratorInitializer<Inventory, DeconstructionInventory>
{
    private static readonly string InventoryComponentName = "DeconstructionInventory";
    private readonly IGoodService _goodService;

    public DeconstructorInventoryInitializer(IGoodService goodService)
    {
        _goodService = goodService;
    }

    public void Initialize(Inventory decorator, DeconstructionInventory subject)
    {
        var buildingCost = subject.GetComponent<Building>().BuildingCost;
        var hasCost = buildingCost is { Count: > 0 };
        if (hasCost)
        {
            var goods = new List<StorableGoodAmount>();
            foreach (var cost in buildingCost)
            {
                var storableGood = StorableGood.CreateAsTakeable(cost.GoodId);
                goods.Add(new StorableGoodAmount(storableGood, cost.Amount));
            }

            var inventoryInitializer = new InventoryInitializer(_goodService, decorator, goods.Sum(good => good.Amount),
                InventoryComponentName);

            inventoryInitializer.AddAllowedGoods(goods);
            inventoryInitializer.HasPublicOutput();
            inventoryInitializer.Initialize();

            AddItems(decorator, goods);
        }
        else
        {
            var inventoryInitializer = new InventoryInitializer(_goodService, decorator, 0, InventoryComponentName);
            inventoryInitializer.Initialize();
        }

        subject.InitializeInventory(decorator);
        //decorator.Disable();
    }

    private void AddItems(Inventory inventory, IEnumerable<StorableGoodAmount> goods)
    {
        foreach (var good in goods) inventory.Give(new GoodAmount(good.StorableGood.GoodId, good.Amount));
    }
}