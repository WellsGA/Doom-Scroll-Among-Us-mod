using System;
using System.Collections.Generic;
using System.Text;
using DoomScroll.Common;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Reactor;

namespace DoomScroll
{
   public sealed class FolderManager
    {
        private static FolderManager _instance;
        public static FolderManager Instance
        {
            get
            {
                if ( _instance == null)
                { 
                    _instance = new FolderManager();
                }
                return _instance;
            }
        }
        public CustomButton FolderToggleBtn { get; private set; }
        public CustomButton CloseBtn { get; private set; }
        public CustomButton HomeBtn { get; private set; }
        public CustomButton BackBtn { get; private set; }
        private bool isFolderOpen;
        private GameObject folderOverlay;

        private FolderManager()
        {
            CreateFolderBtn();
        }
        private void CreateFolderBtn() {
            Vector3 keyboardPosition = HudManager.Instance.Chat.OpenKeyboardButton.transform.parent.position;
            GameObject m_UIParent = HudManager.Instance.Chat.OpenKeyboardButton.transform.parent.gameObject;
            Vector3 position = new Vector3(keyboardPosition.x, keyboardPosition.y, keyboardPosition.z);
            SpriteRenderer sr = HudManager.Instance.Chat.OpenKeyboardButton.GetComponent<SpriteRenderer>();
            Vector2 size = sr.size * sr.transform.localScale;
            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png");

           FolderToggleBtn = new CustomButton(m_UIParent, customButtonSprite, position, size);
    }
        private void InitRootFolder() {
            folderOverlay = new GameObject();

        }
        public void OnClickFolderBtn()
        {
            Logger<DoomScrollPlugin>.Info("Folder clicked");

        }
    }
}
