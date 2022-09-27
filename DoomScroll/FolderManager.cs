
using DoomScroll.Common;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Reactor;

namespace DoomScroll
{
    // Manager class of the folder system 
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
        public Folder Current { get; private set; }
        private Folder previous;
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
            Texture2D tex = ImageLoader.ReadTextureFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderToggle.png");
            
            Sprite defaultBtn = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height/2),
                new Vector2(0.5f, 0.5f)
                );
            Sprite hoveredtBtn = Sprite.Create(
                tex,
                new Rect(0, tex.height / 2, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
                );
            
            FolderToggleBtn = new CustomButton(m_UIParent, defaultBtn, position, size, "FolderToggleButton");
            FolderToggleBtn.ActivateButton(false);
        }

        public void CreateFolderOverlay()
        {
            FolderOverlay = new GameObject();
            FolderOverlay.name = "FolderOverlay";
            FolderOverlay.layer = LayerMask.NameToLayer("UI");
            FolderOverlay.transform.SetParent(m_UIParent.transform);
            FolderOverlay.transform.localPosition = new Vector3(0f, 0f, -10f);
            FolderOverlay.transform.localScale = m_UIParent.transform.localScale * 0.9f;

            SpriteRenderer sr = FolderOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderOverlay.png");
            sr.sprite = spr;


            // add path, back and home buttons

            // deactivate by default
            IsFolderOpen = false;
            FolderOverlay.SetActive(false);
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
            Current = root;
            Logger<DoomScrollPlugin>.Info("Folder structure initiallized");
        }

        private void ToggleFolderOverlay()
        {
            if (IsFolderOpen)
            {
                ActivateFolderOverlay(false);
                Logger<DoomScrollPlugin>.Info("CURRENT FOLDER CLOSED");
            }
            else 
            {
                ActivateFolderOverlay(true);
                Logger<DoomScrollPlugin>.Info("ROOT FOLDER OPEN");
                // (re)sets root as the current folder and displays its content
                Current = root;
                Current.DisplayContent();
                Logger<DoomScrollPlugin>.Info(root.PrintDirectory());
            }

        }

        public void ActivateFolderOverlay(bool value)
        {
            IsFolderOpen = value;
            FolderOverlay.SetActive(value);
        }
        //returns the first item with matching name or null
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
        }

        public void ChangeDirectory(Folder folder) 
        {
            if (FolderToggleBtn.IsActive && IsFolderOpen) 
            {
                previous = Current;
                Current = folder;
                previous.HideContent();
                folder.DisplayContent();
            }
        }

        public void OnClickFolderBtn()
        {
            if (FolderToggleBtn.IsActive)
            {
                ToggleFolderOverlay();
            }
        }
    }
}
