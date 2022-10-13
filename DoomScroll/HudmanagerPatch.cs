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
        private static DoomScrollEvent Cameratoggle = new DoomScrollEvent();
        private static DoomScrollEvent CapturecSreen = new DoomScrollEvent();
        public static DoomScrollEvent FolderToggle = new DoomScrollEvent();
        public static DoomScrollEvent HomeFolder = new DoomScrollEvent();
        public static DoomScrollEvent PrevFolder = new DoomScrollEvent();

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            // subscribe methods to call on buttonclick
            Cameratoggle.MyAction += ScreenshotManager.Instance.OnClickCamera;
            CapturecSreen.MyAction += ScreenshotManager.Instance.OnClickCaptureScreenshot;
            FolderToggle.MyAction += FolderManager.Instance.OnClickFolderBtn;
            HomeFolder.MyAction += FolderManager.Instance.OnClickHomeButton;
            PrevFolder.MyAction += FolderManager.Instance.OnClickBackButton;
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void PostfixUpdate()
        {
            if (!ScreenshotManager.Instance.CameraButton.ButtonGameObject
                || !ScreenshotManager.Instance.CaptureScreenButton.ButtonGameObject)
            { return; }
            
            try
            {
                // Replace sprite on mouse hover for both buttons
                ScreenshotManager.Instance.CameraButton.ReplaceImgageOnHover();
                ScreenshotManager.Instance.CaptureScreenButton.ReplaceImgageOnHover();

                // Invoke methods on mouse click - open camera overlay
                if (ScreenshotManager.Instance.CameraButton.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Cameratoggle.InvokeAction();
                }
                // Invoke methods on mouse click - capture screen
                if (ScreenshotManager.Instance.CaptureScreenButton.isHovered() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    CapturecSreen.InvokeAction();
                }
            }
            catch(Exception e)
            {
                Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
            }
        }

        [HarmonyPostfix]
        // [HarmonyPatch(typeof(HudManager._CoFadeFullScreen_d__66), "MoveNext")]
        [HarmonyPatch("CoFadeFullScreen")]
        public static void PostfixCoFadeFullScreen(HudManager __instance, ref Color target)
        {
            if (__instance.FullScreen.gameObject.activeSelf && __instance.FullScreen.color == target) 
            {
                ScreenshotManager.Instance.CameraButton.ActivateButton(true);
                Logger<DoomScrollPlugin>.Info("HudManager.CoFadeFullScreen ---- CAMERA ACTIVE");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnDestroy")]
        public static void PostfixOnDestroy()
        {
            FolderManager.Instance.ReSet();
            ScreenshotManager.Instance.ReSet();
        }

    }
}
