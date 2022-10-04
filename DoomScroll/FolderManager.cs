
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
        public GameObject pathText;
        public bool IsFolderOverlayOpen { get; private set; }
        public GameObject FolderOverlay { get; private set; }

        private Folder root;
        private Folder previous;
        public Folder Current { get; private set; }
        
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
            Vector3 pos = HudManager.Instance.Chat.OpenKeyboardButton.transform.localPosition;
            SpriteRenderer sr = HudManager.Instance.Chat.OpenKeyboardButton.GetComponent<SpriteRenderer>();
            Vector2 size = sr ? sr.size : new Vector2(0.5f, 0.5f);
            Vector3 position = new(pos.x, pos.y + size.y + 0.1f, pos.z);
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };
            Sprite[] btnSprites = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderToggle.png", slices);
            FolderToggleBtn = new CustomButton(m_UIParent, btnSprites, position, size, "FolderToggleButton");
            FolderToggleBtn.ActivateButton(false);
        }

        public void CreateFolderOverlay()
        {
            Vector4[] slices = { new Vector4(0, 0.5f, 1, 1), new Vector4(0, 0, 1, 0.5f) };

            // create the overlay background
            FolderOverlay = new GameObject();
            FolderOverlay.name = "FolderOverlay";
            FolderOverlay.layer = LayerMask.NameToLayer("UI");
            FolderOverlay.transform.SetParent(m_UIParent.transform);
            FolderOverlay.transform.localPosition = new Vector3(0f, 0f, -10f);
            FolderOverlay.transform.localScale = m_UIParent.transform.localScale * 0.9f;

            SpriteRenderer sr = FolderOverlay.AddComponent<SpriteRenderer>();
            Sprite spr = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderOverlay.png");
            sr.sprite = spr;

            // close button 
            SpriteRenderer sr2 = HudManager.Instance.MapButton;
            Vector2 buttonSize = sr2 ? sr2.size / 1.3f : new Vector2(0.5f, 0.5f);
            Vector3 position = new Vector3(-sr.size.x/2 - buttonSize.x/2 , sr.size.y/2 - buttonSize.y/2, -5f);
            Sprite[] closeBtnImg = { ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.closeButton.png") };
            CloseBtn = new CustomButton(FolderOverlay, closeBtnImg, position, buttonSize, "Close FolderOverlay");

            // home button
            Sprite[] homeBtnImg = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.homeButton.png", slices);
            Vector3 homePosition = new Vector3(-sr.size.x / 2 + buttonSize.x /2, sr.size.y / 2 - buttonSize.y/2, -5f);
            HomeBtn = new CustomButton(FolderOverlay, homeBtnImg, homePosition, buttonSize, "Back to Home");

            // back button
            Sprite[] backBtnImg = ImageLoader.ReadImageSlicesFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.backButton.png", slices);
            Vector3 backPosition = homePosition + new Vector3(buttonSize.x, 0, 0);
            BackBtn = new CustomButton(FolderOverlay, backBtnImg, backPosition, buttonSize, "Back to Previous");

            // path
            /*pathText = new GameObject();
            pathText.name = "PathName";
            pathText.layer = LayerMask.NameToLayer("UI");
            pathText.transform.SetParent(FolderOverlay.transform);
            RectTransform rt = pathText.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(sr.size.x - 2 * buttonSize.x, buttonSize.y);
            new CustomText("path", pathText, Current.GetPath());
*/
            // deactivate by default
            IsFolderOverlayOpen = false;
            FolderOverlay.SetActive(false);
        }
        private void InitFolderStructure() 
        {
            SpriteRenderer sr = FolderOverlay.GetComponent<SpriteRenderer>();
            Sprite folderEmpty = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folderEmpty.png");
            Sprite folder = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.folder.png");

            root = new Folder("", "Home", FolderOverlay, sr, folderEmpty);
            Folder images = new Folder(root.GetPath(), "Images", FolderOverlay, sr, folder);
            Folder tasks = new Folder(root.GetPath(), "Tasks", FolderOverlay, sr, folderEmpty);
            root.AddItem(images);
            root.AddItem(tasks);
            root.AddItem(new Folder(root.GetPath(), "Checkpoints", FolderOverlay, sr, folderEmpty));
           
            Folder subImage = new Folder(images.GetPath(), "SubImages-1", FolderOverlay, sr, folderEmpty);
            images.AddItem(subImage);
            images.AddItem(new Folder(images.GetPath(), "SubImages-2", FolderOverlay, sr, folderEmpty));
            images.AddItem(new Folder(images.GetPath(), "SubImages-3", FolderOverlay, sr, folderEmpty));
            images.AddItem(new Folder(images.GetPath(), "SubImages-4", FolderOverlay, sr, folderEmpty));
            images.AddItem(new Folder(images.GetPath(), "SubImages-5", FolderOverlay, sr, folderEmpty));
            images.AddItem(new Folder(images.GetPath(), "SubImages-6", FolderOverlay, sr, folderEmpty));
            images.AddItem(new Folder(images.GetPath(), "SubImages-7", FolderOverlay, sr, folderEmpty));

            subImage.AddItem(new Folder(subImage.GetPath(), "Sub-SubImages-1", FolderOverlay, sr, folder));
            subImage.AddItem(new Folder(subImage.GetPath(), "Sub-SubImages-2", FolderOverlay, sr, folder));

            tasks.AddItem(new Folder(tasks.GetPath(), "SubTasks-1", FolderOverlay, sr, folderEmpty));
            tasks.AddItem(new Folder(tasks.GetPath(), "SubTasks-2", FolderOverlay, sr, folderEmpty));
            tasks.AddItem(new Folder(tasks.GetPath(), "SubTasks-3", FolderOverlay, sr, folderEmpty));
            tasks.AddItem(new Folder(tasks.GetPath(), "SubTasks-4", FolderOverlay, sr, folderEmpty));

            Current = root;
            previous = root;
            Logger<DoomScrollPlugin>.Info("Folder structure initiallized");
        }

        private void ToggleFolderOverlay()
        {
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
                Logger<DoomScrollPlugin>.Info(root.PrintDirectory());
            }

        }

        public void ActivateFolderOverlay(bool value)
        {
            IsFolderOverlayOpen = value;
            FolderOverlay.SetActive(value);
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
    }
}
