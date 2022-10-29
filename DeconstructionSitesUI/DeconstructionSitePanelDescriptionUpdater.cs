using Timberborn.EntityPanelSystem;
using Timberborn.SingletonSystem;

namespace RecyclableDestruction.DeconstructionSitesUI;

/*public class DeconstructionSitePanelDescriptionUpdater : ILoadableSingleton
{
    private readonly IEntityPanel _entityPanel;
    private readonly EventBus _eventBus;

    public DeconstructionSitePanelDescriptionUpdater(IEntityPanel entityPanel, EventBus eventBus)
    {
        this._entityPanel = entityPanel;
        this._eventBus = eventBus;
    }

    public void Load() => this._eventBus.Register((object) this);

    [OnEvent]
    public void OnConstructibleEnteredFinishedState(
        DeconstructibleEnteredFinishedStateEvent constructibleEnteredFinishedStateEvent)
    {
        _entityPanel. .ReloadDescription(constructibleEnteredFinishedStateEvent.Constructible.gameObject);
    }
}*/