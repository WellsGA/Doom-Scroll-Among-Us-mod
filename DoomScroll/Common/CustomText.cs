using System;
using System.Collections;
using System.Text;
using UnityEngine;
using TMPro;

namespace DoomScroll.Common
{
    public class CustomText
    {
        public GameObject TextObject { get; }
        public TextMeshPro TextMP { get; }
        private MeshRenderer m_meshRenderer;

        public CustomText(string name, GameObject parent, string text) 
        {
            TextObject = new GameObject();
            TextObject.layer = LayerMask.NameToLayer("UI");
            TextObject.name = name;
            TextObject.transform.SetParent(parent.transform, true);

            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            Vector3 position = parent.transform.localPosition;
            float yOffset = sr ? sr.size.y/2 + 0.1f : 0;
            TextObject.transform.position = position;
            TextObject.transform.localPosition = new Vector3(position.x, position.y - yOffset, position.z);
            
            m_meshRenderer = TextObject.AddComponent<MeshRenderer>();

            m_meshRenderer.transform.localScale = parent.transform.localScale;

            TextMP = TextObject.AddComponent<TextMeshPro>();
            TextMP.text = text;
            TextMP.m_enableWordWrapping = true;
            TextMP.alignment = TextAlignmentOptions.Center;

            // TextMP.fontSize = 25;
            TextMP.color = Color.black;
  
            // TextMP.outlineColor = Color.black;
            //TextMP.SetOutlineThickness(0.1f);
        }
    }
}