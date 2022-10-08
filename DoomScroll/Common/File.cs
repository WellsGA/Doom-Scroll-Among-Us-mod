using UnityEngine;
using DoomScroll.UI;

namespace DoomScroll.Common
{
    public enum FileType 
    {
        IMAGE,
        MAPSOURCE
    }
    public class File : IDirectory
    {
        // type?? image and mapsource ...

        private string name;
        private string path;
        private CustomButton fileBtn;
        private FileType type;
        private byte[] content;

        private GameObject parentUI = FolderManager.Instance.FolderHolder;
        public File(string parentPath, string name, byte[] image)
        {
            this.name = name;
            path = parentPath + "/" + name;
            content = image;
            Sprite[] images = { ImageLoader.ReadImageFromByteArray(content) };
            
            fileBtn = new CustomButton(parentUI, images, parentUI.transform.position, parentUI.GetComponent<SpriteRenderer>().size / 5, name);
            new CustomText(name, fileBtn.ButtonGameObject, name);
        }
        public string GetName()
        {
            return name;
        }
        public string GetPath()
        {
            return path;
        }
        public CustomButton GetButton() 
        {
            return fileBtn;
        }
        public void DisplayContent() 
        {
            // display the content of the file 
            switch (type) 
            {
                case FileType.IMAGE:

                    return;
                case FileType.MAPSOURCE:
                    return;
            }
        }
        public void HideContent()
        {
            // hide the content -- close the overlay
        }
        public string PrintDirectory()
        {
            return " " + path + " [file]";
        }
    }
}
