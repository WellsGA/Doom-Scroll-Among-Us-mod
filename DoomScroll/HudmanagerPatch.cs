using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Reactor;


namespace DoomScroll
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        public static event Action cameraToggle;
        public static event Action capturecSreen;
       
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            // subscribe methods to call on buttonclick
            cameraToggle += ScreenshotManager.Instance.OnClickCamera;
            capturecSreen += ScreenshotManager.Instance.OnClickCaptureScreenshot;
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
                    cameraToggle?.Invoke();
                }
                if (ScreenshotManager.Instance.CaptureScreenButton.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    capturecSreen?.Invoke();
                }
            }
            catch(Exception e)
            {
                Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
            }
        }

        

    }
}
