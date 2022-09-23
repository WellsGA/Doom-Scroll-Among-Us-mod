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
        private RectTransform m_rectTransform;
        private MeshRenderer m_meshRenderer;

        public CustomText(string name, GameObject parent, Vector3 position, string text) 
        {
            TextObject = new GameObject();
            TextObject.layer = LayerMask.NameToLayer("UI");
            TextObject.name = name;
            
            m_rectTransform = TextObject.AddComponent<RectTransform>();
            m_rectTransform.SetParent(parent.transform, true);

            SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
            float xOffSet = sr ? sr.size.x /2 + 0.3f : 0;
            float yOffset = sr ? sr.size.y/2 + 0.3f : 0;
            m_rectTransform.transform.position = position;
            m_rectTransform.transform.localPosition = new Vector3(position.x - xOffSet, position.y - yOffset, position.z);
            
            m_meshRenderer = TextObject.AddComponent<MeshRenderer>();

            m_meshRenderer.transform.localScale = parent.transform.localScale / 0.8f;

            TextMP = TextObject.AddComponent<TextMeshPro>();
            TextMP.text = text;
            TextMP.m_enableWordWrapping = true;
           // TextMP.alignment = TextAlignmentOptions.Center;
          

            // TextMP.fontSize = 25;
            TextMP.color = Color.black;
  
            // TextMP.outlineColor = Color.black;
            //TextMP.SetOutlineThickness(0.1f);
        }
    }
}
