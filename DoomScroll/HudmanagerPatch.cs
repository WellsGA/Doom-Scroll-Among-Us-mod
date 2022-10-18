using System;
using HarmonyLib;
using UnityEngine;
using Reactor;
using DoomScroll.Common;


namespace DoomScroll
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            ScreenshotManager.Instance.ReSet();
            FolderManager.Instance.Reset();           
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void PostfixUpdate(HudManager __instance)
        {
            ScreenshotManager.Instance.CheckButtonClicks();
            __instance.TaskText.text += "\nSWC: " + SecondaryWinConditionHolder.getSomePlayerSWC(PlayerControl.LocalPlayer._cachedData.PlayerId).SWCAssignText();
        }

        [HarmonyPostfix]
        [HarmonyPatch("SetHudActive")]
        public static void PostfixSetHudActive(bool isActive)
        {
            if (!ScreenshotManager.Instance.IsCameraOpen)
            {
                ScreenshotManager.Instance.ActivateCameraButton(isActive);
                Logger<DoomScrollPlugin>.Info("HudManager.SetHudActive ---- CAMERA ACTIVE" + isActive);
            }
        }

    }
}
 