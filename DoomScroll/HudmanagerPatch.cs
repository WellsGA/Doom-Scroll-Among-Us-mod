using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Reactor;
using System.Reflection;


namespace DoomScroll
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        public static event Action m_listener;
        
        private static CustomButton m_cameraButton;
        private static GameObject m_UIParent;


        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix(HudManager __instance)
        {
            // Create custom screenshot button
            m_UIParent = __instance.gameObject;
            Vector3 mapBtnPos = __instance.MapButton.gameObject.transform.position;
            Vector3 position = new Vector3(mapBtnPos.x, mapBtnPos.y - __instance.MapButton.size.y * __instance.MapButton.transform.localScale.y, mapBtnPos.z);
            Vector2 size = __instance.MapButton.size * __instance.MapButton.transform.localScale;

            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png");
            m_cameraButton = new CustomButton(m_UIParent, customButtonSprite, position, size);

            // sadly this doesn't work... // m_buttonGo.m_Button.onClick.AddListener(m_listener);

            // subscribe method to call on buttonclick
            m_listener += OnClickCamera;
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void Postfix()
        {
            try
            {
                // Invoke the subscribing methods on mouse click 
                if (m_cameraButton.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_listener?.Invoke();
                }
                if (ScreenshotManager.Instance.CaptureScreenButton.IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    OnClickCaptureScreenshot();
                }
            }
            catch(Exception e)
            {
                Logger<DoomScrollPlugin>.Error("Error invoking method: " + e);
            }
        }

        static void OnClickCamera()
        {
            ScreenshotManager.Instance.ToggleCamera();
        }

        static void OnClickCaptureScreenshot()
        {
            ScreenshotManager.Instance.CaptureScreenshot();
        }

    }
}
