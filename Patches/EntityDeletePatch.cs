using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RecyclableDestruction.Configurators;
using RecyclableDestruction.DeconstructionSites;
using RecyclableDestruction.Types;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;

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
        RecyclableDestruction.LOGGER.LogInfo($"POSTFIX {nameof(EntityDeletePatch)}");
        __state = blockObjects;
        foreach (var blockObject in blockObjects)
        {
            blockObject.TryGetComponent(out BuildingModel buildingModel);
            blockObject.TryGetComponent(out DeconstructionSite deconstructionSite);
            if (deconstructionSite == null) return true;
            var deconstructable = blockObject.GetComponent<Deconstructable>();
            if (deconstructable.IsDeconstructing) return false;
            deconstructable.Deconstruct();
            /*constructible.EnterConstructionState(ConstructionState.Unfinished);
            constructible.enabled = false;
            buildingModel.ShowUnfinishedModel();
            blockObject.MarkAsUnfinished();
            deconstructionInventory.EnableInventory(blockObject.gameObject);*/
        }

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