using System;
using Reactor;
using HarmonyLib;
using UnityEngine;

namespace DoomScroll
{
    [HarmonyPatch(typeof(MeetingHud))]
    static class MeetingHudPatch
    {
        
        [HarmonyPostfix]
        [HarmonyPatch("Start")]  
        public static void PostfixStart()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(false);
        }

        [HarmonyPostfix]
        [HarmonyPatch("HandleProceed")]
        public static void PostfixHandleProceed()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(true);
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void PostfixUpdate()
        {
            if (HudManager.Instance.Chat.IsOpen && !FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(true);
                Logger<DoomScrollPlugin>.Info("ACTIVE ");
            }
            else if (!HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(false);
                // hide overlay if it was still open
                if (FolderManager.Instance.IsFolderOpen)
                {
                    FolderManager.Instance.ActivateFolderOverlay(false);
                    Logger<DoomScrollPlugin>.Info("OVERLAY DEACTIVE ");
                }
                Logger<DoomScrollPlugin>.Info("INACTIVE ");
            }
            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                try
                {          
                    // Invoke FolderToggle on mouse click 
                    if (FolderManager.Instance.FolderToggleBtn.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.FolderToggle.InvokeAction();
                    }

                }
                catch (Exception e)
                {
                    Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
                }
            }
        }
    }
}
