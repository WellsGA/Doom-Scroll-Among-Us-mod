using UnityEngine;
using DoomScroll.Common;
using System.Reflection;

namespace DoomScroll.UI
{
    static class ScreenshotOverlay
    {
        public static CustomButton CreateCameraButton(HudManager hud)
        {
            GameObject m_UIParent = hud.gameObject;
            Vector3 mapBtnPos = hud.MapButton.transform.position;
            Vector3 position = new Vector3(mapBtnPos.x, mapBtnPos.y - hud.MapButton.size.y * hud.MapButton.transform.localScale.y, mapBtnPos.z);
            Vector2 scaledSize = hud.MapButton.size * HudManager.Instance.MapButton.transform.localScale;
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] cameraBtnSprites = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png", slices);

            return new CustomButton(m_UIParent, cameraBtnSprites, position, scaledSize, "Camera Toggle Button");
        }

        public static CustomButton CreateCaptureButton(GameObject parent) 
        {
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] captureSprite = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.captureScreenNew.png", slices);
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector3 pos = new Vector3(sr.size.x / 2 - 0.7f, 0, -10);

            return new CustomButton(parent, captureSprite, pos, new Vector2(0.6f, 0.6f), "Screenshot Button");
        }

        public static GameObject InitCameraOverlay(HudManager hud) 
        {
            GameObject cameraOverlay = new GameObject();
            cameraOverlay.name = "ScreenshotOverlay";
            cameraOverlay.layer = LayerMask.NameToLayer("UI");
            cameraOverlay.transform.SetParent(hud.transform);
            cameraOverlay.transform.localPosition = new Vector3(0f, 0f, -5);

            SpriteRenderer sr = cameraOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraOverlay.png");
            sr.sprite = spr;

            // deactivate by default
            cameraOverlay.SetActive(false);
            return cameraOverlay;
        }
    }
}
