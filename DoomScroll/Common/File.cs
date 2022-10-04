using System;
using System.Collections.Generic;
using System.Reflection;
using Reactor;
using UnityEngine;
using TMPro;

namespace DoomScroll.Common
{
    public class File : IDirectory
    {
        // type?? image and mapsource ...

        private string name;
        private string path;
        private CustomButton fileBtn;

        public File(string parentPath, string name, GameObject parent, SpriteRenderer sr)
        {
            this.name = name;
            path = parentPath + "/" + name;
            Sprite[] images = { ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.file.png") };
            fileBtn = new CustomButton(parent, images, parent.transform.position, sr.size / 5, name);
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
