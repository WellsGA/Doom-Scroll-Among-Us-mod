using UnityEngine;
using Reactor;
using System.Reflection;
using DoomScroll.Common;
using DoomScroll.UI;

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
        private  HudManager hudManagerInstance;
        private GameObject UIOverlay;
        public CustomButton CameraButton { get; private set; }
        public CustomButton CaptureScreenButton { get; private set; }

        private Camera mainCamrea;
        private int m_screenshots;
        private int m_maxPictures;
        private bool m_isCameraOpen;
        private ScreenshotManager() 
        {
            hudManagerInstance = HudManager.Instance;
            mainCamrea = Camera.main;
            m_screenshots = 0;
            m_maxPictures = 3;
            m_isCameraOpen = false;

            CameraButton = ScreenshotOverlay.CreateCameraButton(hudManagerInstance);
            UIOverlay = ScreenshotOverlay.InitCameraOverlay(hudManagerInstance);
            CaptureScreenButton = ScreenshotOverlay.CreateCaptureButton(UIOverlay);
           
            CameraButton.ActivateButton(false);
            Logger<DoomScrollPlugin>.Info("SCREENSHOT MANAGER CONSTRUCTOR");
        }

        private void CaptureScreenshot()
        {  
            if (mainCamrea)
            {
                // hide player and overlay
                ShowOverlays(false);

                // use the main camera to render screen into a texture
                RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 20);
                mainCamrea.targetTexture = screenTexture;
                RenderTexture.active = screenTexture;
                mainCamrea.Render();
                
                Texture2D screeenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                screeenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                // screeenShot.Apply();

                // reset camera, show player and overlay
                // RenderTexture.active = null;
                screenTexture.Release();
                mainCamrea.targetTexture = null;
                Object.Destroy(screenTexture);
                
                ShowOverlays(true);

                byte[] byteArray = screeenShot.EncodeToPNG();
                // save the image locally -- for testing purposes
                System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture_" + m_screenshots + ".png", byteArray);

                // save the in the inventory folder
                FolderManager.Instance.AddImageToScreenshots("image_" + m_screenshots + ".png", byteArray);

                Object.Destroy(screeenShot);
                m_screenshots++;
                Logger<DoomScrollPlugin>.Info("number of screenshots: " + m_screenshots);
            }

        }

        private void ShowOverlays(bool value)
        {
            PlayerControl.LocalPlayer.gameObject.SetActive(value);
            UIOverlay.SetActive(value);
        }

        public void ToggleCamera()
        {
            if (!UIOverlay) { return; }
            if (m_isCameraOpen)
            {
                UIOverlay.SetActive(false);
                CaptureScreenButton.EnableButton(false);
                m_isCameraOpen = false;
                HudManager.Instance.SetHudActive(true);
            }
            else
            {
                UIOverlay.SetActive(true);
                CaptureScreenButton.EnableButton(true);
                m_isCameraOpen = true;
                HudManager.Instance.SetHudActive(false);
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
                CameraButton.EnableButton(false);
            }
        }

        public void ReSet()
        {   
            m_screenshots = 0;
            m_isCameraOpen = false;
            _instance = null;
            Logger<DoomScrollPlugin>.Info("SCREENSHOT MANAGER RESET");
        }

    }
}
