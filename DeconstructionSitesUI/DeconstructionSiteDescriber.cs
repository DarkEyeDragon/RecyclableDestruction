using Bindito.Core;
using Timberborn.ConstructionSites;
using Timberborn.Localization;
using UnityEngine;

namespace RecyclableDestruction.DeconstructionSitesUI;

public class DeconstructionSiteDescriber : MonoBehaviour
{
    private static readonly string ProgressLocKey = "ConstructionSites.Progress";
    private static readonly string WaitingForMaterialsLocKey = "ConstructionSites.Info.WaitingForMaterials";
    private ILoc _loc;
    private ConstructionSite _constructionSite;

    [Inject]
    public void InjectDependencies(ILoc loc) => _loc = loc;

    public void Awake() => _constructionSite = GetComponent<ConstructionSite>();

    public string GetProgressInfoShort() => string.Format("{0:0}% {1}", (float) (_constructionSite.BuildTimeProgress * 100.0), GetAdditionalInfo());

    public string GetProgressInfoFull()
    {
        string str = string.Format("{0:0}", (float) (_constructionSite.BuildTimeProgress * 100.0));
        return _loc.T(ProgressLocKey, str) + " " + GetAdditionalInfo();
    }

    private string GetAdditionalInfo() => _constructionSite.MaterialProgress < 1.0 && !_constructionSite.HasMaterialsToResumeBuilding ? " " + _loc.T(WaitingForMaterialsLocKey) : "";
}