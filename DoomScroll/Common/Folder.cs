using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection;
using Reactor;

namespace DoomScroll.Common
{
    public class Folder : IDirectory
    {    
        public List<IDirectory> Content { get; private set; }
       
        private string name;
        private string path;
        private CustomButton folderBtn;

        private Sprite folderImg;
        private GameObject parentUI;
        private SpriteRenderer spriteRndr;
        public Folder(string name, GameObject parent, SpriteRenderer sr, Sprite image)
        {
            this.name = name;
            path = name;
            parentUI = parent;
            spriteRndr = sr;
            Content = new List<IDirectory>();
            folderImg = image;
            Sprite[] images = { folderImg };
            folderBtn = new CustomButton(parentUI, images, parentUI.transform.position, sr.size/5 - new Vector2(0.2f, 0.2f), name);
            new CustomText(name, folderBtn.ButtonGameObject, name);
        }

        public void AddItem(IDirectory item)
        {
            Content.Add(item); // new item with initial path
            SetChildPath(path, item); // set correct path
        }

        public void RemoveItem(IDirectory item)
        {
            Content.Remove(item);
        }

        public string GetName()
        {
            return name;
        }

        public CustomButton GetButton()
        {
            return folderBtn;
        }
        private void SetChildPath(string path, IDirectory item)
        {
            item.SetPath(path);
        }
        public void SetPath(string path) {
            this.path = path + "/" + name;
        }
        public void DisplayContent() 
        {
 
            Vector3 pos = new Vector3(0f, 0f, -20f);
            float width = spriteRndr.size.x;
            float height = spriteRndr.size.y;

            // display items on a 5x5 grid 
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(i+j < Content.Count)
                    {
                        pos.x = i * width / 5 - width/2 + 0.7f;
                        pos.y = j * height / 5 + height/2 - 0.7f;
                        Content[j+i].GetButton().SetLocalPosition(pos);
                        Content[j+i].GetButton().ActivateButton(true);
                    }
                }
            }
        }
        public void HideContent() 
        {
            foreach (IDirectory dir in Content) 
            {
                dir.GetButton().ActivateButton(false);
            }
        }
        public string PrintDirectory()
        {
            string items = "";
            foreach (IDirectory dir in Content)
            {
                items += dir.PrintDirectory();
            }
            return path + "[ " + items + " ]";
        }

        public void OnClickFolder() 
        {

        }
    }
}
