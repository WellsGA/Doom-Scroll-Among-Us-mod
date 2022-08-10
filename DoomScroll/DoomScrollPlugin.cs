﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace DoomScroll;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class DoomScrollPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {
        ConfigName = Config.Bind("Fake", "Name", ":>");

        Harmony.PatchAll();
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class ExamplePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            __instance.cosmetics.nameText.text = PluginSingleton<DoomScrollPlugin>.Instance.ConfigName.Value;
        }
    }
}