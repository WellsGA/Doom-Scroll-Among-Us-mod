
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
        
        public bool IsFolderOpen { get; private set; }
        public GameObject FolderOverlay { get; private set; }
       // public GameObject FolderOverlayInner { get; private set; }

        private Folder root;
        private Folder current;

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
            FolderOverlay = new GameObject();
            FolderOverlay.name = "FolderOverlay";
            FolderOverlay.layer = LayerMask.NameToLayer("UI");
            RectTransform rectTransform = FolderOverlay.AddComponent<RectTransform>();
            rectTransform.SetParent(m_UIParent.transform);
            rectTransform.transform.localPosition = new Vector3(0f, 0f, -10f);

            SpriteRenderer sr = FolderOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderOverlay.png");
            sr.sprite = spr;

            // add back and home buttons TO DO

            // deactivate by default
            IsFolderOpen = false;
            FolderOverlay.SetActive(false);
            Logger<DoomScrollPlugin>.Info("Folder overlay created, size " + sr.size.ToString() );
        }
        private void InitFolderStructure() 
        {
            SpriteRenderer sr = FolderOverlay.GetComponent<SpriteRenderer>();
            Sprite folderEmpty = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderEmpty.png");
            Sprite folder = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folder.png");

            root = new Folder("Home", FolderOverlay, sr, folderEmpty);
            
            root.AddItem(new Folder("Images", FolderOverlay, sr, folder));
            root.AddItem(new Folder("Tasks", FolderOverlay, sr, folderEmpty));
            root.AddItem(new Folder("Checkpoints", FolderOverlay, sr, folderEmpty));
            
            Logger<DoomScrollPlugin>.Info("Folder structure initiallized");
        }

        private void ToggleFolderOverlay()
        {
            if (IsFolderOpen)
            {
                ActivateFolderOverlay(false);
                Logger<DoomScrollPlugin>.Info("ROOT FOLDER CLOSED");
            }
            else 
            {
                ActivateFolderOverlay(true);
                Logger<DoomScrollPlugin>.Info("ROOT FOLDER OPEN");
                root.DisplayContent();
                Logger<DoomScrollPlugin>.Info(root.PrintDirectory());
            }

        }

        public void ActivateFolderOverlay(bool value)
        {
            IsFolderOpen = value;
            FolderOverlay.SetActive(value);
        }
       /* //returns the first item with matching name or null
        public Folder GetFolderByName(string name, Folder folder) 
        {
            foreach (IDirectory dir in folder.Content) 
            {
                if (dir.GetName().Equals(name) && dir.GetType().Equals(folder))
                {
                    return (Folder)dir;
                }
            }
            return null;
        }*/
        public void OnClickFolderBtn()
        {
            if (FolderToggleBtn.IsActive)
            {
                ToggleFolderOverlay();
            }
            
        }
    }
}
