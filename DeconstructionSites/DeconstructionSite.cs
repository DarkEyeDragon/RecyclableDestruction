using System;
using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using RecyclableDestruction.Configurators;
using RecyclableDestruction.Types;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.ConstructionSites;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.Localization;
using Timberborn.StatusSystem;
using Timberborn.TickSystem;

namespace RecyclableDestruction.DeconstructionSites;

public class DeconstructionSite : TickableComponent, IRegisteredComponent, IUnfinishedStateListener, IPausableComponent
{
    private IBlockOccupancyService _blockOccupancyService;
    private ILoc _loc;
    private BlockObject _blockObject;
    private Deconstructable _deconstructible;
    private Building _building;
    //private GroundedConstructionSite _groundedConstructionSite;
    private BlockableBuilding _blockableBuilding;
    private StatusToggle _lackOfResourcesStatusToggle;
    private bool _isReservedForBuild;
    public DeconstructionInventory Inventory { get; private set; }

    [Inject]
    public void InjectDependencies(IBlockOccupancyService blockOccupancyService, ILoc loc)
    {
        _blockOccupancyService = blockOccupancyService;
        _loc = loc;
    }

    public void Awake()
    {
        _blockObject = GetComponent<BlockObject>();
        _deconstructible = GetComponent<Deconstructable>();
        //_constructionTime = GetComponent<IConstructionTime>();
        _building = GetComponent<Building>();
        //_groundedConstructionSite = GetComponent<GroundedConstructionSite>();
        _blockableBuilding = GetComponent<BlockableBuilding>();
        _lackOfResourcesStatusToggle = StatusToggle.CreateNormalStatusWithAlert("LackOfResources", _loc.T(ConstructionSite.NoMaterialsLocKey), _loc.T(ConstructionSite.NoMaterialsShortLocKey), 3f);
        enabled = false;
    }
    //public override void StartTickable() => GetComponent<StatusSubject>().RegisterStatus(_lackOfResourcesStatusToggle);
    
    public override void Tick() => FinishIfRequirementsMet();
    
    public IEnumerable<GoodAmount> RemainingRequiredGoods() => Inventory.Inventory.AllowedGoods.Select(good =>
    {
        string goodId = good.StorableGood.GoodId;
        int amount = Inventory.Inventory.UnreservedCapacity(goodId);
        return new GoodAmount(goodId, amount);
    }).Where(good => good.Amount > 0);
    
    public void ReserveForBuild() => _isReservedForBuild = true;

    public void UnreserveForBuild() => _isReservedForBuild = false;

    public void OnEnterUnfinishedState()
    {
        enabled = true;
        Inventory.Inventory.Enable();
    }

    public void OnExitUnfinishedState()
    {
        enabled = false;
        Inventory.Inventory.Disable();
    }
    public void InitializeInventory(DeconstructionInventory inventory)
    {
        Asserts.FieldIsNull(this, Inventory, "Inventory");
        Inventory = inventory;
    }
    private void FinishIfRequirementsMet()
    {
        if (Inventory.HasBuildingMaterials || BlockedByBeaversOnSite())
            return;
        _deconstructible.Deconstruct();
    }
    private bool BlockedByBeaversOnSite() => !_building.FinishableWithBeaversOnSite && _blockOccupancyService.OccupantPresentOnArea(_blockObject, 0.0f);

}