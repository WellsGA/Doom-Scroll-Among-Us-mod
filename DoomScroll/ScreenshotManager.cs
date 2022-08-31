using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Reactor;
using System.Reflection;

namespace DoomScroll
{
    // a manager class for handleing screenshots
    class ScreenshotManager
    {

        private static ScreenshotManager _instance;
        public static ScreenshotManager Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new ScreenshotManager();
                }
                return _instance;
            }
        
        }

        private bool isCameraOpen;
        
        private GameObject cameraOverlay;

        private ScreenshotManager() 
        {
            isCameraOpen = false;
            cameraOverlay = new GameObject();
            cameraOverlay.name = "ScreenshotOverlay";
            RectTransform rectTransform = cameraOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(DestroyableSingleton<HudManager>.Instance.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -5);
            
            SpriteRenderer sr = cameraOverlay.AddComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, 0.5f); // make it transparent
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraOverlay.png");
            sr.sprite = spr;
            
        }

        public void ToggleCamera() 
        {
            if (isCameraOpen)
            {
                cameraOverlay.SetActive(false);
                isCameraOpen = false;
                DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
            }
            else 
            {
                cameraOverlay.SetActive(true);
                isCameraOpen = true;
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            }
            Logger<DoomScrollPlugin>.Info("CAMERA BUTTON active: " + isCameraOpen);
        }

    }
}
