using UnityEngine;
using Reactor;
using System.Reflection;
using DoomScroll.Common;

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
           
            CreateCameraButton();
            InitScreenshotOverlay();
        }

        private void ToggleCamera() 
        {  
            if (m_isCameraOpen)
            {
                m_cameraOverlay.SetActive(false);
                CaptureScreenButton.EnableButton(false);
                m_isCameraOpen = false;
                hudManagerInsance.SetHudActive(true);
            }
            else 
            {
                m_cameraOverlay.SetActive(true);
                CaptureScreenButton.EnableButton(true);
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
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] cameraBtnSprites = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png", slices);

            CameraButton = new CustomButton(m_UIParent, cameraBtnSprites, position, size, "Camera Toggle Button");
        }

        private void InitScreenshotOverlay()
        {
            m_cameraOverlay = new GameObject();
            m_cameraOverlay.name = "ScreenshotOverlay";
            m_cameraOverlay.layer = LayerMask.NameToLayer("UI");
            RectTransform rectTransform = m_cameraOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(hudManagerInsance.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -5);

            SpriteRenderer sr = m_cameraOverlay.AddComponent<SpriteRenderer>();
            // sr.color = new Color(1f, 1f, 1f, 0.5f); // make it transparent
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraOverlay.png");
            sr.sprite = spr;

            // add the capture button
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] captureSprite = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.captureScreenNew.png", slices);
            Vector3 pos = new Vector3(sr.size.x / 2 - 0.7f, 0, -10);
            
            CaptureScreenButton = new CustomButton(m_cameraOverlay, captureSprite, pos, new Vector2(0.5f, 0.5f), "Screenshot Button");

            // deactivate by default
            m_isCameraOpen = false;
            m_cameraOverlay.SetActive(false);
        }

        private void CaptureScreenshot()
        {  
            // check for null refernce 
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



                Object.Destroy(screeenShot);

                m_screenshots++;
                Logger<DoomScrollPlugin>.Info("number of screenshots: " + m_screenshots);
            }

        }

        private void ShowOverlays(bool value)
        {
            PlayerControl.LocalPlayer.gameObject.SetActive(value);
            m_cameraOverlay.SetActive(value);
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
    }
}
