using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace DoomScroll
{
    // Creates and manages custom buttnos
    public class CustomButton
    {
        private GameObject m_buttonGo;
        public Button m_Button { get; private set; }
        public SpriteRenderer m_Spriterenderer { get; private set; }
        private RectTransform m_rectTransform;

     

        public CustomButton(Vector3 position, GameObject parent, Sprite buttonImg, Vector2 size)
        {
            m_buttonGo = new GameObject();
            m_Button = m_buttonGo.AddComponent<Button>();

            m_rectTransform = m_buttonGo.AddComponent<RectTransform>();
            m_rectTransform.SetParent(parent.transform, false);
            m_rectTransform.transform.localPosition = position;

            m_Spriterenderer = m_buttonGo.AddComponent<SpriteRenderer>();
            m_Spriterenderer.sprite = buttonImg;
            
            float scale = size.x / m_Spriterenderer.size.x;
            m_Spriterenderer.transform.localScale *= scale;

            // debug: Logger<DoomScrollPlugin>.Info(" sprite renderer size: " + m_spriterenderer.size);
        }

        
    }
}
