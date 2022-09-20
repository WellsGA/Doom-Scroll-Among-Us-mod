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
    // a manager class of the folder system attached to the the meeting chat
    // basic singleton pattern - not thread safe
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

        GameObject m_UIParent;
        public CustomButton FolderToggleBtn { get; set; }
        public CustomButton CloseBtn { get; private set; }
        public CustomButton HomeBtn { get; private set; }
        public CustomButton BackBtn { get; private set; }
        
        private bool isFolderOpen;
        private GameObject folderOverlay;

        private FolderManager()
        {
            // setting Chat UI as the parent gameobject
            m_UIParent = HudManager.Instance.Chat.OpenKeyboardButton.transform.parent.gameObject;
            CreateFolderBtn();
            CreateFolderOverlay();
            InitFolderStructure();
        }
        private void CreateFolderBtn() 
        {  
            Vector3 pos = HudManager.Instance.Chat.OpenKeyboardButton.transform.position;
            SpriteRenderer sr = HudManager.Instance.Chat.OpenKeyboardButton.GetComponent<SpriteRenderer>();
            Vector2 size = sr ? sr.size - new Vector2(0.05f, 0.05f) : new Vector2(0.5f, 0.5f);
            Vector3 position = new(pos.x, pos.y + size.y + 0.1f, pos.z);
            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderToggle.png");

            FolderToggleBtn = new CustomButton(m_UIParent, customButtonSprite, position, size, "FolderToggleButton");
            FolderToggleBtn.ActivateButton(false);
        }

        public void CreateFolderOverlay()
        {
            folderOverlay = new GameObject();
            folderOverlay.name = "FolderOverlay";
            folderOverlay.layer = LayerMask.NameToLayer("UI");
            RectTransform rectTransform = folderOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(m_UIParent.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -10f);

            SpriteRenderer sr = folderOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderOverlay.png");
            sr.sprite = spr;

            // add back and home buttons TO DO

            // deactivate by default
            isFolderOpen = false;
            folderOverlay.SetActive(false);
            Logger<DoomScrollPlugin>.Info("Folder overlay size " + sr.sprite.rect.size.ToString() );
        }
        private void InitFolderStructure() 
        {    
            Logger<DoomScrollPlugin>.Info("Folder structure initiallized");
        }
        private void ToggleFolderOverlay()
        {
            if (isFolderOpen)
            {
                isFolderOpen = false;
                folderOverlay.SetActive(false);
            }
            else 
            {
                isFolderOpen = true;
                folderOverlay.SetActive(true);
            }

        }
        public void OnClickFolderBtn()
        {
            if (FolderToggleBtn.IsEnabled && FolderToggleBtn.IsActive)
            {
                ToggleFolderOverlay();
                Logger<DoomScrollPlugin>.Info("Folder clicked");
            }
            
        }
    }
}
