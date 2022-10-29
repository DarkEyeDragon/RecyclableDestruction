using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.InventorySystem;
using Timberborn.InventorySystemUI;
using UnityEngine.UIElements;

namespace RecyclableDestruction.DeconstructionSitesUI;

public class DeconstructionSiteFragmentInventory
{
    private readonly InformationalRowsFactory _informationalRowsFactory;
    private Inventory _inventory;
    private VisualElement _inventoryRoot;
    private ScrollView _inventoryContent;
    private readonly List<InformationalRow> _rows = new List<InformationalRow>();

    public DeconstructionSiteFragmentInventory(InformationalRowsFactory informationalRowsFactory) =>
        _informationalRowsFactory = informationalRowsFactory;

    public void InitializeFragment(VisualElement root)
    {
        _inventoryRoot = root.Q<VisualElement>("DeconstructionSiteInventoryFragment");
        _inventoryContent = _inventoryRoot.Q<ScrollView>("Content");
    }

    public void ShowFragment(Inventory inventory)
    {
        _inventory = inventory;
        _rows.AddRange(_informationalRowsFactory.CreateRowsWithLimits(_inventory, _inventoryContent));
    }

    public void ClearFragment()
    {
        _inventoryContent.Clear();
        _rows.Clear();
        _inventory = null;
    }

    public void UpdateFragment()
    {
        if (_inventory && _inventory.enabled)
        {
            _inventoryRoot.ToggleDisplayStyle(_rows.Count > 0);
            foreach (InformationalRow row in _rows)
                row.ShowUpdated();
        }
        else
            _inventoryRoot.ToggleDisplayStyle(false);
    }
}