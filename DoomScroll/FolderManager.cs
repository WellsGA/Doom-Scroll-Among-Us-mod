
using DoomScroll.Common;
using UnityEngine;
using DoomScroll.UI;
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
        public CustomText pathText;
        public bool IsFolderOverlayOpen { get; private set; }
        public GameObject FolderHolder { get; private set; }

        private Folder root;
        private Folder previous;
        public Folder Current { get; private set; }
        private Folder screenshots; 
        public Folder Tasks { get; private set; }
        private FolderManager()
        {
            // setting Chat UI as the parent gameobject
            m_UIParent = HudManager.Instance.Chat.OpenKeyboardButton.transform.parent.gameObject;
            IsFolderOverlayOpen = false;
            InitFolderStructure();
            FolderToggleBtn = FolderOverlay.CreateFolderBtn(m_UIParent);
            FolderHolder = FolderOverlay.CreateFolderOverlay(m_UIParent);
            CloseBtn = FolderOverlay.AddCloseButton(FolderHolder);
            HomeBtn = FolderOverlay.AddHomeButton(FolderHolder);
            BackBtn = FolderOverlay.AddBackButton(FolderHolder);
           
            pathText = new CustomText("path", FolderOverlay.AddPath(FolderHolder), Current.GetPath());

            Logger<DoomScrollPlugin>.Info("Folder manager initiallized");
        }
        
        private void InitFolderStructure() 
        {
            Sprite folderEmpty = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderEmpty.png");

            root = new Folder("", "Home", FolderHolder, folderEmpty);
            screenshots = new Folder(root.GetPath(), "Images", FolderHolder, folderEmpty);
            Tasks = new Folder(root.GetPath(), "Tasks", FolderHolder, folderEmpty);
            root.AddItem(screenshots);
            root.AddItem(Tasks);
            root.AddItem(new Folder(root.GetPath(), "Checkpoints", FolderHolder, folderEmpty));

            Current = root;
            previous = root;
        }

        private void ToggleFolderOverlay()
        {
            if (!FolderHolder) { return; }
            if (IsFolderOverlayOpen)
            {
                ActivateFolderOverlay(false);
                Current.HideContent();
                Logger<DoomScrollPlugin>.Info("CURRENT FOLDER CLOSED");
            }
            else 
            {
                ActivateFolderOverlay(true);
                Logger<DoomScrollPlugin>.Info("ROOT FOLDER OPEN");
                // (re)sets root as the current and previous folder and displays its content
                Current = root;
                previous = root;
                Current.DisplayContent();
            }

        }
        public void AddImageToScreenshots(string name, byte[] img) 
        {
            screenshots.AddItem(new File(screenshots.GetPath(), name, img));
        }
        public void ActivateFolderOverlay(bool value)
        {
            IsFolderOverlayOpen = value;
            FolderHolder.SetActive(value);
        }

        public void ChangeDirectory(Folder folder) 
        {
            if (FolderToggleBtn.IsActive && IsFolderOverlayOpen) 
            {
                previous = Current;
                Current = folder;
                previous.HideContent();
                Current.DisplayContent();
            }
        }

        public void OnClickFolderBtn()
        {
            if (FolderToggleBtn.IsActive)
            {
                ToggleFolderOverlay();
            }
        }

        public void OnClickHomeButton() 
        {
            ChangeDirectory(root);
        }

        public void OnClickBackButton()
        {
            ChangeDirectory(previous);
        }

        public void ReSet() 
        {
           _instance = null;
        }
    }
}
