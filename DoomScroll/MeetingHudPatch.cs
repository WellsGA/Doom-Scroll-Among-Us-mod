using System;
using System.Collections.Generic;
using System.Reflection;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(MeetingHud))]
    static class MeetingHudPatch
    {

        private static DoomScrollEvent FolderToggle = new DoomScrollEvent();
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
       
        public static void PostfixStart()
        {
            // subscribe methods to call on buttonclick
            FolderToggle.MyAction += FolderManager.Instance.OnClickFolderBtn;
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
                Logger<DoomScrollPlugin>.Info("INACTIVE ");
            }
            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                try
                {
                   
                    // Invoke methods on mouse click 
                    if (FolderManager.Instance.FolderToggleBtn.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        FolderToggle.InvokeAction();
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
