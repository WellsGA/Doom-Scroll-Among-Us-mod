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
    // basic singleton pattern - not thread safe
    public sealed class ScreenshotManager
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

        private HudManager hudManagerInsance;
        private Camera mainCamrea;
        private GameObject m_cameraOverlay;
        private int m_screenshots;
        private bool m_isCameraOpen;
        private int m_maxPictures;
        public CustomButton CaptureScreenButton { get; private set; }
        public CustomButton CameraButton { get; private set; }

        private ScreenshotManager() 
        {
            m_screenshots = 0;
            m_maxPictures = 3;
            // get HudManager Instance and main Camera
            hudManagerInsance = DestroyableSingleton<HudManager>.Instance;
            mainCamrea = hudManagerInsance.GetComponentInParent<Camera>();
            // create camera button
            CreateCameraButton();
            // init screenshot overlay
            InitScreenshotOverlay();
        }

        private void ToggleCamera() 
        {  
            if (m_isCameraOpen)
            {
                m_cameraOverlay.SetActive(false);
                CaptureScreenButton.ActivateButton(false);
                m_isCameraOpen = false;
                hudManagerInsance.SetHudActive(true);
            }
            else 
            {
                m_cameraOverlay.SetActive(true);
                CaptureScreenButton.ActivateButton(true);
                m_isCameraOpen = true;
                hudManagerInsance.SetHudActive(false);
            }
            Logger<DoomScrollPlugin>.Info("Camera overlay active: " + m_isCameraOpen);
        }


        private void CreateCameraButton() 
        {
            GameObject m_UIParent = hudManagerInsance.gameObject;
            Vector3 mapBtnPos = hudManagerInsance.MapButton.gameObject.transform.position;
            Vector3 position = new Vector3(mapBtnPos.x, mapBtnPos.y - hudManagerInsance.MapButton.size.y * hudManagerInsance.MapButton.transform.localScale.y, mapBtnPos.z);
            Vector2 size = hudManagerInsance.MapButton.size * hudManagerInsance.MapButton.transform.localScale;
            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png");
            
            CameraButton = new CustomButton(m_UIParent, customButtonSprite, position, size);
        }

        private void InitScreenshotOverlay()
        {
            m_cameraOverlay = new GameObject();
            m_cameraOverlay.name = "ScreenshotOverlay";
            RectTransform rectTransform = m_cameraOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(hudManagerInsance.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -5);

            SpriteRenderer sr = m_cameraOverlay.AddComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, 0.5f); // make it transparent
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraOverlay.png");
            sr.sprite = spr;

            // add the capture button
            Sprite captureSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.captureScreen.png");
            Vector3 pos = new Vector3(sr.size.x / 2 - 0.7f, 0, -25);
            
            CaptureScreenButton = new CustomButton(m_cameraOverlay, captureSprite, pos, new Vector2(0.5f, 0.5f));

            // deactivate by default
            m_isCameraOpen = false;
            m_cameraOverlay.SetActive(false);
        }

        // This could be a coroutine!!
        private void CaptureScreenshot()
        {  
            // check for null refernce 
            if (mainCamrea)
            {
                // hide player
                PlayerControl.LocalPlayer.gameObject.SetActive(false);

                // use the main camera to render screen into a texture
                RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 20);
                mainCamrea.targetTexture = screenTexture;
                RenderTexture.active = screenTexture;
                mainCamrea.Render();
                Texture2D screeenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                screeenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                
                // reset camera and player
                PlayerControl.LocalPlayer.gameObject.SetActive(true);
                RenderTexture.active = null;
                mainCamrea.targetTexture = null;
                UnityEngine.Object.Destroy(screenTexture);
                
                // save the image locally -- for now... this must be changed!
                byte[] byteArray = screeenShot.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture_" + m_screenshots + ".png", byteArray);
                
                m_screenshots++;
                Logger<DoomScrollPlugin>.Info("number of screenshots: " + m_screenshots);
            }

        }

        // methods subscribing to the button click events 
        public void OnClickCamera()
        {
            ToggleCamera();
        }

        public void OnClickCaptureScreenshot()
        {
            CaptureScreenshot();
            ToggleCamera();

            if (m_screenshots == m_maxPictures)
            {
                CameraButton.ActivateButton(false);
            }
        }
    }
}
