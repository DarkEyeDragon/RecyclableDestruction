using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RecyclableDestruction.Configurators;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.InventorySystem;
using Timberborn.SimpleOutputBuildings;

namespace RecyclableDestruction.Patches;

[HarmonyPatch]
public class EntityDeletePatch
{
    public static MethodInfo TargetMethod()
    {
        return AccessTools.Method(AccessTools.TypeByName("AnyBlockObjectDeletionTool"), "DeleteBlockObjects",
            new[] { typeof(IEnumerable<BlockObject>) });
    }

static bool Prefix(IEnumerable<BlockObject> blockObjects, out IEnumerable<BlockObject> __state)
{
    RecyclableDestruction.LOGGER.LogInfo($"POSTFIX {nameof(__state)}");
    foreach (var blockObject in blockObjects)
    {
        blockObject.TryGetComponent(out BuildingModel buildingModel);
        blockObject.TryGetComponent(out Building building);
        blockObject.TryGetComponent(out DeconstructionInventory deconstructionInventory);
        if (deconstructionInventory == null) break;
        /*foreach (var goodAmountSpecification in building.BuildingCost)
        {
            deconstructionInventory.Inventory.Give(goodAmountSpecification.ToGoodAmount());
        }*/
        buildingModel.ShowUnfinishedModel();
        blockObject.MarkAsUnfinished();
        RecyclableDestruction.LOGGER.LogInfo(deconstructionInventory.Inventory.Stock.Count());
    }
    __state = blockObjects;
    return false;
}

    static void Postfix(IEnumerable<BlockObject> __state)
    {
        /*RecyclableDestruction.LOGGER.LogInfo($"POSTFIX {nameof(__state)}");
        foreach (var blockObject in __state)
        {
            blockObject.TryGetComponent(out Building building);
            blockObject.TryGetComponent(out BuildingModel buildingModel);
            buildingModel.ShowUnfinishedModel();
            RecyclableDestruction.LOGGER.LogInfo(building.BuildingCost);
        }*/
    }
}