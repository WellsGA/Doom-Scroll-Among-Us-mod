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

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            // subscribe methods to call on buttonclick
            Cameratoggle.MyAction += ScreenshotManager.Instance.OnClickCamera;
            CapturecSreen.MyAction += ScreenshotManager.Instance.OnClickCaptureScreenshot;
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void PostfixUpdate()
        {
            try
            {
                // Invoke methods on mouse click 
                if (ScreenshotManager.Instance.CameraButton.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Cameratoggle.InvokeAction();
                }
                if (ScreenshotManager.Instance.CaptureScreenButton.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    CapturecSreen.InvokeAction();
                }
            }
            catch(Exception e)
            {
                Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
            }
        }

        

    }
}
