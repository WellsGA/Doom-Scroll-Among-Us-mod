using System;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

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
            // Change button icon on hover
            FolderManager.Instance.FolderToggleBtn.ReplaceImgageOnHover();

            // Activate FolderToggle Button if Chat is open and hide if it's closed
            if (HudManager.Instance.Chat.IsOpen && !FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(true);
                Logger<DoomScrollPlugin>.Info("ACTIVE ");
            }
            else if (!HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(false);
                // hide overlay too if it was still open
                if (FolderManager.Instance.IsFolderOpen)
                {
                    FolderManager.Instance.ActivateFolderOverlay(false);
                }
                Logger<DoomScrollPlugin>.Info("INACTIVE ");
            }

            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                try
                {          
                    // Invoke FolderToggle on mouse click 
                    if (FolderManager.Instance.FolderToggleBtn.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.FolderToggle.InvokeAction();
                    }

                }
                catch (Exception e)
                {
                    Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
                }
            }

            // Check if any of the displayed folders are clicked (probably could be handled with events...)
            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.IsFolderOpen)
            {
                try
                {
                    foreach (IDirectory dir in FolderManager.Instance.Current.Content)
                    {
                        if (dir.GetButton().isHovered() && Input.GetKey(KeyCode.Mouse0)) 
                        {
                            if (dir is Folder folder) 
                            {
                                FolderManager.Instance.ChangeDirectory(folder);
                            }
                        }
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
