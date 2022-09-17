using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RecyclableDestruction.Configurators;
using TimberbornAPI;

namespace RecyclableDestruction
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class RecyclableDestruction : BaseUnityPlugin
    {
        public static ManualLogSource LOGGER;

        private void Awake()
        {
            // Plugin startup logic
            LOGGER = Logger;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            var harmony = new Harmony("me.darkeyedragon.recyclabledestruction");
            TimberAPI.DependencyRegistry.AddConfigurator(new DeconstructionInventoryConfigurator());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
