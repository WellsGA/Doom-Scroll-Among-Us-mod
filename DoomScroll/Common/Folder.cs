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
        public Folder ParentFolder { get; }
        
        private string name;
        private string path;
        private CustomButton folderBtn;

        private GameObject parentUI;
        private SpriteRenderer spriteRndr;
        public Folder(string parentPath, string name, GameObject parent, SpriteRenderer sr, Sprite folderImg)
        {
            this.name = name;
            path = parentPath + "/" + name;
            parentUI = parent;
            spriteRndr = sr;
            Content = new List<IDirectory>();
            Sprite[] images = { folderImg };
            folderBtn = new CustomButton(parentUI, images, parentUI.transform.position, sr.size/5 - new Vector2(0.2f, 0.2f), name);  
            new CustomText(name, folderBtn.ButtonGameObject, name);
            folderBtn.ActivateButton(false);
        }

        public void AddItem(IDirectory item)
        {
            Content.Add(item);
        }

        public void RemoveItem(IDirectory item)
        {
            Content.Remove(item);
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
            return folderBtn;
        }
      
        public void DisplayContent() 
        {
 
            Vector3 pos = new Vector3(0f, 0f, -20f);
            float width = spriteRndr.size.x;
            float height = spriteRndr.size.y - 1f;

            // display items on a 5x5 grid 
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(j+i*5 < Content.Count)
                    {
                        pos.x = j * width / 5 - width/2 + 0.7f;
                        pos.y = i * -height / 5 + height/2 - 0.7f;
                        Content[j+i*5].GetButton().SetLocalPosition(pos);
                        Content[j+i*5].GetButton().ActivateButton(true);
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

    }
}
