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

        
        private GameObject m_cameraOverlay;
        private int m_screenshots = 0;
        private bool isCameraOpen;
        public CustomButton CaptureScreenButton { get; private set; }

        private ScreenshotManager() 
        {
            // set up the overlay
            m_cameraOverlay = new GameObject();
            m_cameraOverlay.name = "ScreenshotOverlay";
            RectTransform rectTransform = m_cameraOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(DestroyableSingleton<HudManager>.Instance.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -5);
           
            SpriteRenderer sr = m_cameraOverlay.AddComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, 0.5f); // make it transparent
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraOverlay.png");
            sr.sprite = spr;

            // create capture button
            Sprite captureSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.captureScreen.png");
            Vector3 pos = new Vector3(sr.size.x / 2 - 0.7f, 0, -25);
            CaptureScreenButton = new CustomButton(m_cameraOverlay, captureSprite, pos, new Vector2(0.5f, 0.5f));

            isCameraOpen = false;
            m_cameraOverlay.SetActive(false);
        }

        public void ToggleCamera() 
        {
            if (isCameraOpen)
            {
                m_cameraOverlay.SetActive(false);
                isCameraOpen = false;
                DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
            }
            else 
            {
                m_cameraOverlay.SetActive(true);
                isCameraOpen = true;
                DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            }
            Logger<DoomScrollPlugin>.Info("Camera overlay active: " + isCameraOpen);
        }

        public void CaptureScreenshot()
        {
            m_screenshots++;
            Logger<DoomScrollPlugin>.Info("number of screenshots: " + m_screenshots);
        }
    }
}
