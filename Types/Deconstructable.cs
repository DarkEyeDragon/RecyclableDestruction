using System;
using Bindito.Core;
using RecyclableDestruction.Events;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace RecyclableDestruction.Types;

public class Deconstructable : MonoBehaviour, IPersistentEntity, IInitializableEntity, IDeletableEntity,
    IRegisteredComponent
{
    private EventBus _eventBus;
    private static readonly ComponentKey DeconstructibleKey = new(nameof(Deconstructable));
    private static readonly PropertyKey<bool> FinishedKey = new("Finished");
    private bool _initialized;
    public bool IsDeconstructing { get; private set; }

    [Inject]
    public void InjectDependencies(EventBus eventBus) => _eventBus = eventBus;

    public void Save(IEntitySaver entitySaver) => entitySaver.GetComponent(DeconstructibleKey)
        .Set(FinishedKey, IsDeconstructing);

    public void Load(IEntityLoader entityLoader)
    {
        //IsDeconstructing = entityLoader.GetComponent(DeconstructibleKey).GetValueOrNullable(FinishedKey) ?? false;
        IsDeconstructing = false;
    }

    public void InitializeEntity()
    {
        try
        {
            NotifyOnStateChanged();
            _initialized = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Exception while initializing a constructible: " + gameObject.name, ex);
        }
    }

    public void DeleteEntity()
    {
        throw new NotImplementedException();
    }

    public void Deconstruct()
    {
        if (IsDeconstructing)
        {
            throw new InvalidOperationException($"{gameObject.name} is already being deconstructed!");
        }
        NotifyOnStateChanged();
    }

    private void NotifyOnStateChanged()
    {
        if (!_initialized) return;
        if (IsDeconstructing)
        {
            foreach (IUnfinishedStateListener componentsInChild in
                     GetComponentsInChildren<IUnfinishedStateListener>(true))
                componentsInChild.OnEnterUnfinishedState();
            _eventBus.Post(new DeconstructedEvent(this));
        }
        else
        {
            foreach (IFinishedStateListener componentsInChild in GetComponentsInChildren<IFinishedStateListener>(true))
                componentsInChild.OnEnterFinishedState();
            _eventBus.Post(new DeconstructedCancelEvent(this));
        }
    }
}