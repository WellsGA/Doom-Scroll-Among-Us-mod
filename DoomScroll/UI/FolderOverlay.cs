using DoomScroll.Common;
using System.Reflection;
using UnityEngine;

namespace DoomScroll.UI
{
    public static class FolderOverlay
    {
        public static CustomButton CreateFolderBtn(GameObject parent)
        {
            GameObject keyboardBtn = HudManager.Instance.Chat.OpenKeyboardButton;
            Vector3 pos = keyboardBtn.transform.localPosition;
            SpriteRenderer sr = keyboardBtn.GetComponent<SpriteRenderer>();
            Vector2 size = sr ? sr.size : new Vector2(0.5f, 0.5f);
            Vector3 position = new(pos.x, pos.y + size.y + 0.1f, pos.z);
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] btnSprites = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderToggle.png", slices);
            CustomButton folderBtn = new CustomButton(parent, btnSprites, position, size, "FolderToggleButton");
            folderBtn.ActivateButton(false);
            return folderBtn;
        }

        public static GameObject CreateFolderOverlay(GameObject parent)
        {
            // create the overlay background
            GameObject folderOverlay = new GameObject();
            folderOverlay.name = "FolderOverlay";
            folderOverlay.layer = LayerMask.NameToLayer("UI");
            folderOverlay.transform.SetParent(parent.transform);
            folderOverlay.transform.localPosition = new Vector3(0f, 0f, -10f);
            folderOverlay.transform.localScale = parent.transform.localScale * 0.4f;

            SpriteRenderer sr = folderOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderOverlay.png");
            sr.sprite = spr;
            folderOverlay.SetActive(false);
            return folderOverlay;
        }

        public static CustomButton AddCloseButton(GameObject parent)
        {
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector2 buttonSize = new Vector2(0.4f, 0.4f);
            Vector3 position = new Vector3(-sr.size.x / 2 - buttonSize.x, sr.size.y / 2 - buttonSize.y, -5f);
            Sprite[] closeBtnImg = { ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.closeButton.png") };
            return new CustomButton(parent, closeBtnImg, position, buttonSize, "Close FolderOverlay");      
            // deactivate by default
            
        }
        public static CustomButton AddHomeButton(GameObject parent)
        {
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Vector2 buttonSize = new Vector2(0.4f, 0.4f);
            Sprite[] homeBtnImg = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.homeButton.png", slices);
            Vector3 homePosition = new Vector3(-sr.size.x / 2 + buttonSize.x, sr.size.y / 2 - buttonSize.y, -5f);
            return new CustomButton(parent, homeBtnImg, homePosition, buttonSize, "Back to Home");

        }
        public static CustomButton AddBackButton(GameObject parent)
        {
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Vector2 buttonSize = new Vector2(0.4f, 0.4f);
            Sprite[] backBtnImg = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.backButton.png", slices);
            Vector3 backPosition = new Vector3(-sr.size.x / 2 + buttonSize.x * 2, sr.size.y / 2 - buttonSize.y, -5f);
            return new CustomButton(parent, backBtnImg, backPosition, buttonSize, "Back to Previous");

        }
        public static GameObject AddPath(GameObject parent) 
        {
            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector2 buttonSize = new Vector2(0.4f, 0.4f);

            GameObject pathText = new GameObject();
            pathText.name = "PathName";
            pathText.layer = LayerMask.NameToLayer("UI");
            pathText.transform.SetParent(parent.transform);
            RectTransform rt = pathText.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(sr.size.x - 3 * buttonSize.x, buttonSize.y);
            return pathText;
        }
    }
}
