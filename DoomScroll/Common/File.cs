﻿using UnityEngine;
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
        private GameObject parentUI;
        public File(string parentPath, GameObject parent, string name, byte[] image)
        {
            this.name = name;
            path = parentPath + "/" + name;
            parentUI = parent;
            content = image;
            // float width = parentUI.GetComponent<SpriteRenderer>()? parentUI.GetComponent<SpriteRenderer>().size.x / 5 : 0.8f;
            Sprite[] images = { ImageLoader.ReadImageFromByteArray(content) };       
            fileBtn = new CustomButton(parentUI, images, parentUI.transform.position, 0.8f, name);
            new CustomText(name, fileBtn.ButtonGameObject, name);
            fileBtn.ActivateButton(false);
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
            // display the content of the file -- TO DO
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
