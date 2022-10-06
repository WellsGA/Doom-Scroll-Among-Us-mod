using System;
using System.Collections.Generic;
using System.Reflection;
using Reactor;
using UnityEngine;
using TMPro;

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

        private GameObject parent = FolderManager.Instance.FolderOverlay;
        public File(string parentPath, string name, byte[] image)
        {
            this.name = name;
            path = parentPath + "/" + name;
            content = image;
            Sprite[] images = { ImageLoader.ReadImageFromByteArray(content) };
            
            fileBtn = new CustomButton(parent, images, parent.transform.position, parent.GetComponent<SpriteRenderer>().size / 5, name);
            new CustomText(name, fileBtn.ButtonGameObject, name);
        }
        public string GetName()
        {
            return name;
        }
        public string GetPath()
        {
            return name;
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
