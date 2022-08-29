using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace DoomScroll { 

    [BepInAutoPlugin]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public partial class DoomScrollPlugin : BasePlugin
    {
        public Harmony Harmony { get; } = new(Id);

        public override void Load()
        {
            Logger<DoomScrollPlugin>.Info(" ---------  DoomScroll Plugin Loaded ---------");
            Harmony.PatchAll();
            HudManagerPatch.InitHudManager();

        }

    }

}