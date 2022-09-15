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
                if (_instance == null)
                {
                    _instance = new FolderManager();
                }
                return _instance;
            }
        }
        public CustomButton FolderToggleBtn { get; set; }
        public CustomButton CloseBtn { get; private set; }
        public CustomButton HomeBtn { get; private set; }
        public CustomButton BackBtn { get; private set; }
        
        private bool isFolderOpen;
        private GameObject folderOverlay;

        private FolderManager()
        {
            CreateFolderBtn();
            InitFolderStructure();
        }
        private void CreateFolderBtn() 
        {
            // setting Chat UI as the parent gameobject
            GameObject m_UIParent = HudManager.Instance.Chat.OpenKeyboardButton.transform.parent.gameObject;
            Vector3 pos = HudManager.Instance.Chat.OpenKeyboardButton.transform.position;

            SpriteRenderer sr = HudManager.Instance.Chat.OpenKeyboardButton.GetComponent<SpriteRenderer>();
            Vector2 size = sr ? sr.size - new Vector2(0.05f, 0.05f) : new Vector2(0.5f, 0.5f);
            Vector3 position = new(pos.x, pos.y + size.y + 0.1f, pos.z);
            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderToggle.png");

            FolderToggleBtn = new CustomButton(m_UIParent, customButtonSprite, position, size, "Folder Toggle Button");
            FolderToggleBtn.ActivateButton(false);

        }
        private void InitFolderStructure() 
        {    
            Logger<DoomScrollPlugin>.Info(" Folder structure initiallized");
        }

        public void InitFolderOverlay() { }

        public void OnClickFolderBtn()
        {
            if (FolderToggleBtn.IsEnabled && FolderToggleBtn.IsActive)
            {
                Logger<DoomScrollPlugin>.Info("Folder clicked");
            }
            
        }
    }
}
