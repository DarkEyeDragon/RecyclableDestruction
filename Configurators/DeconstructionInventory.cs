using System;
using Bindito.Core;
using RecyclableDestruction.Events;
using RecyclableDestruction.Types;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace RecyclableDestruction.Configurators;

public class DeconstructionInventory : MonoBehaviour
{
    [SerializeField]
    private int _capacity;

    private EventBus _eventBus;
    private GameObject _gameObject;
    public Inventory Inventory { get; private set; }

    public int Capacity => _capacity;

    public bool HasBuildingMaterials => Inventory.Capacity > 0;
    public void Awake()
    {
        enabled = false;
    }

    [Inject]
    public void InjectDependencies(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public void EnableInventory(GameObject gameObject)
    {
        //if (Inventory == null) return;
        enabled = true;
        Inventory.Enable();
        Inventory.InventoryChanged += (_1, _2) => DeleteIfEmpty();
        _gameObject = gameObject;
    }
    
    public void InitializeInventory(Inventory inventory)
    {
        //Asserts.FieldIsNull(this, Inventory, "Inventory");
        if (Inventory != null)
        {
            RecyclableDestruction.LOGGER.LogInfo($"InitializeInventory: {Inventory.name}");
        }
        Inventory = inventory;
    }

    private void DeleteIfEmpty()
    {
        RecyclableDestruction.LOGGER.LogInfo($"DeleteIfEmpty");
        if (!Inventory.IsEmpty)
            return;
        Delete();
    }

    private void Delete()
    {
        RecyclableDestruction.LOGGER.LogInfo("DELETE");
        enabled = false;
    }
}