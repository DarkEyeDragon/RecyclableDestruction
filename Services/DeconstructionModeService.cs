using System;
using RecyclableDestruction.Events;
using Timberborn.EntitySystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using Timberborn.WaterSystemRendering;

namespace RecyclableDestruction.Services;

public class DeconstructionModeService : ILoadableSingleton
{
    private readonly EventBus _eventBus;
    private readonly EntityService _entityService;
    private readonly SelectionManager _selectionManager;
    private readonly ToolGroupManager _toolGroupManager;
    private readonly WaterOpacityService _waterOpacityService;
    private WaterOpacityToggle _waterOpacityToggle;

    public bool InConstructionMode { get; private set; }

    public DeconstructionModeService(
        EventBus eventBus,
        EntityService entityService,
        SelectionManager selectionManager,
        ToolGroupManager toolGroupManager,
        WaterOpacityService waterOpacityService)
    {
        _eventBus = eventBus;
        _entityService = entityService;
        _selectionManager = selectionManager;
        _toolGroupManager = toolGroupManager;
        _waterOpacityService = waterOpacityService;
    }

    public void Load()
    {
        _eventBus.Register(this);
        _waterOpacityToggle = _waterOpacityService.GetWaterOpacityToggle();
    }
    
    [OnEvent]
    public void OnDeconstructedEvent(DeconstructedEvent e)
    {
        _entityService.Delete(e.Deconstructable.gameObject);
    }

    [OnEvent]
    public void OnDeconstructedCanceledEvent(DeconstructedCancelEvent cancelEvent)
    {
        throw new NotImplementedException();
    }
}