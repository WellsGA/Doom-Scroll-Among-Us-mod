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

        private Sprite fileImg;
        private CustomButton fileBtn;
        private CustomText fileText;
        public File(string name, GameObject parent, SpriteRenderer sr)
        {
            this.name = name;
            path = name;
           
            fileImg = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.file.png");
            fileBtn = new CustomButton(parent, fileImg, parent.transform.position, sr.size / 5, name);

            fileText = new CustomText(name, fileBtn.ButtonGameObject, fileBtn.ButtonGameObject.transform.position, name);
        }
        public string GetName()
        {
            return name;
        }

        public void SetPath(string path)
        {
            this.path = path + "/" + name;
        }
        public CustomButton GetButton() 
        {
            return fileBtn;
        }
        public void DisplayContent() 
        {
            // display the content of the file
            
        }

        public string PrintDirectory()
        {
            return " " + path + " [file]";
        }
    }
}
