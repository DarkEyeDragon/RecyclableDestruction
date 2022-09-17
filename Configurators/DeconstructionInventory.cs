using System;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.InventorySystem;
using UnityEngine;

namespace RecyclableDestruction.Configurators;

public class DeconstructionInventory : MonoBehaviour, IFinishedStateListener
{
    [SerializeField]
    private int _capacity;
    [SerializeField]
    private bool _disableAfterEmptying;

    public Inventory Inventory { get; private set; }

    public int Capacity => _capacity;

    public void Awake() => enabled = false;

    public void InitializeInventory(Inventory inventory)
    {
        Asserts.FieldIsNull(this, Inventory, "Inventory");
        Inventory = inventory;
    }

    public void OnEnterFinishedState()
    {
        if (Inventory.IsEmpty)
            return;
        enabled = true;
        Inventory.Enable();
        Inventory.InventoryChanged += (EventHandler<InventoryChangedEventArgs>) ((_1, _2) => DisableIfEmpty());
    }

    public void OnExitFinishedState() => Disable();

    private void DisableIfEmpty()
    {
        if (!Inventory.IsEmpty)
            return;
        Disable();
    }

    private void Disable()
    {
        Inventory.Disable();
        enabled = false;
    }
}