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
        [HarmonyPatch("Update")]
        public static void PostfixUpdate()
        {
            // Change button icon on hover
            FolderManager.Instance.FolderToggleBtn.ReplaceImgageOnHover();
            FolderManager.Instance.HomeBtn.ReplaceImgageOnHover();
            FolderManager.Instance.BackBtn.ReplaceImgageOnHover();

            // Activate FolderToggle Button if Chat is open and hide if it's closed
            if (HudManager.Instance.Chat.IsOpen && !FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(true);
                Logger<DoomScrollPlugin>.Info("ACTIVE ");
            }
            else if (!HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                FolderManager.Instance.FolderToggleBtn.ActivateButton(false);
                // hide overlay and current folders too if it was still open
                if (FolderManager.Instance.IsFolderOverlayOpen)
                {
                    HudManagerPatch.FolderToggle.InvokeAction();
                }
                Logger<DoomScrollPlugin>.Info("INACTIVE ");
            }

            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.FolderToggleBtn.IsActive)
            {
                try
                {          
                    // Invoke FolderToggle on mouse click 
                    if (FolderManager.Instance.FolderToggleBtn.isHovered()  && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.FolderToggle.InvokeAction();
                    }

                }
                catch (Exception e)
                {
                    Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
                }
            }

            
            if (HudManager.Instance.Chat.IsOpen && FolderManager.Instance.IsFolderOverlayOpen)
            {
                try
                {
                    if ( FolderManager.Instance.CloseBtn.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.FolderToggle.InvokeAction();
                    }
                    if (FolderManager.Instance.HomeBtn.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.HomeFolder.InvokeAction();
                    }
                    if (FolderManager.Instance.BackBtn.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        HudManagerPatch.PrevFolder.InvokeAction();
                    }
                    // Check if any of the displayed folders are clicked 
                    foreach (IDirectory dir in FolderManager.Instance.Current.Content)
                    {
                        if (dir.GetButton().isHovered() && Input.GetKeyUp(KeyCode.Mouse0)) 
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

        /* [HarmonyPostfix]
         [HarmonyPatch("Close")]
         public static void PostfixClose()
         {
             ScreenshotManager.Instance.CameraButton.ActivateButton(true);
             Logger<DoomScrollPlugin>.Info("MeetingHud.Close ---- CAMERA ACTIVE");
         }*/
    }
}
