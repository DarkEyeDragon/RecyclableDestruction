using System;
using System.Collections.Generic;
using RecyclableDestruction.DeconstructionSites;
using Timberborn.ConstructionSitesUI;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.PrioritySystem;
using Timberborn.PrioritySystemUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace RecyclableDestruction.DeconstructionSitesUI;

public class DeconstructionSiteFragment : IEntityPanelFragment
{
    private static readonly string PriorityLocKey = "ConstructionSites.Priority";
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ConstructionSiteFragmentInventory _deconstructionSiteFragmentInventory;
    private readonly PriorityToggleFactory _priorityToggleFactory;
    private readonly PrioritizableHighlighter _prioritizableHighlighter;
    private readonly ITooltipRegistrar _tooltipRegistrar;
    private VisualElement _root;
    private VisualElement _progress;
    private Label _description;
    private readonly List<PriorityToggle> _toggles = new();
    private Prioritizable _prioritizable;
    private DeconstructionSite _deconstructionSite;
    private DeconstructionSiteDescriber _deconstructionSiteDescriber;

    public DeconstructionSiteFragment(
        VisualElementLoader visualElementLoader,
        ConstructionSiteFragmentInventory deconstructionSiteFragmentInventory,
        PriorityToggleFactory priorityToggleFactory,
        PrioritizableHighlighter prioritizableHighlighter,
        ITooltipRegistrar tooltipRegistrar)
    {
        _visualElementLoader = visualElementLoader;
        _deconstructionSiteFragmentInventory = deconstructionSiteFragmentInventory;
        _priorityToggleFactory = priorityToggleFactory;
        _prioritizableHighlighter = prioritizableHighlighter;
        _tooltipRegistrar = tooltipRegistrar;
    }


    public VisualElement InitializeFragment()
    {
        _root = _visualElementLoader.LoadVisualElement("Master/EntityPanel/ConstructionSiteFragment");
        _progress = _root.Q<VisualElement>("Progress");
        _description = _root.Q<Label>("Text");
        _deconstructionSiteFragmentInventory.InitializeFragment(_root);
        VisualElement visualElement = _root.Q<VisualElement>("Priorities");
        _tooltipRegistrar.RegisterLocalizable(visualElement, PriorityLocKey);
        foreach (Priority priority1 in Priorities.Ascending)
        {
            Priority priority = priority1;
            _toggles.Add(_priorityToggleFactory.Create(priority,
                (Func<bool>)(() => _prioritizable.Priority == priority),
                (Action<bool>)(value => SetPriority(priority, _prioritizable, value)), visualElement));
        }

        _root.ToggleDisplayStyle(false);
        return _root;
    }

    public void ShowFragment(GameObject entity)
    {
        _deconstructionSite = entity.GetComponent<DeconstructionSite>();
        if (!_deconstructionSite)
            return;
        _deconstructionSiteDescriber = _deconstructionSite.GetComponent<DeconstructionSiteDescriber>();
        _prioritizable = entity.GetComponent<Prioritizable>();
        _deconstructionSiteFragmentInventory.ShowFragment(_deconstructionSite.Inventory.Inventory);
    }

    public void ClearFragment()
    {
        _deconstructionSite = null;
        _deconstructionSiteDescriber = null;
    }

    public void UpdateFragment()
    {
        if (_deconstructionSite && _deconstructionSite.enabled)
        {
            _description.text = _deconstructionSiteDescriber.GetProgressInfoShort();
            _deconstructionSiteFragmentInventory.UpdateFragment();
            _progress.style.width = new StyleLength(Length.Percent(/*_deconstructionSite.BuildTimeProgress*/ 1 * 100));
            foreach (PriorityToggle toggle in _toggles)
                toggle.UpdateState();
            _root.ToggleDisplayStyle(true);
        }
        else
            _root.ToggleDisplayStyle(false);
    }
    
    private void SetPriority(Priority priority, Prioritizable prioritizable, bool value)
    {
        if (!((bool) (UnityEngine.Object) prioritizable & value))
            return;
        prioritizable.SetPriority(priority);
        _prioritizableHighlighter.HighlightIfEnabled();
    }
}