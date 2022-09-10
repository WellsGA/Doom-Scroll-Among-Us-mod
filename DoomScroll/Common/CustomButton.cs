using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace DoomScroll.Common
{
    // Creates and manages custom buttnos
    public class CustomButton
    {
        private GameObject m_buttonGo;
        private RectTransform m_rectTransform;
        private SpriteRenderer m_spriterenderer;
        public float CoolDown { get; set; }
        private bool isActive;

        public CustomButton(GameObject parent, Sprite buttonImg, Vector3 position, Vector2 size)
        {
            m_buttonGo = new GameObject();
            m_buttonGo.AddComponent<Button>();

            m_rectTransform = m_buttonGo.AddComponent<RectTransform>();
            m_rectTransform.SetParent(parent.transform, false);
            m_rectTransform.transform.localPosition = position;

            m_spriterenderer = m_buttonGo.AddComponent<SpriteRenderer>();
            m_spriterenderer.sprite = buttonImg;
            
            float scale = size.x / m_spriterenderer.size.x;
            m_spriterenderer.transform.localScale *= scale;
            
            ActivateButton(true);
            // debug: Logger<DoomScrollPlugin>.Info(" sprite renderer size: " + m_spriterenderer.size);
        }

        public bool IsClicked()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 btnPos = m_spriterenderer.transform.position;
            Vector3 btnScale = m_spriterenderer.bounds.extents;

            bool isInBoundsX = btnPos.x - btnScale.x < mousePos.x && btnPos.x + btnScale.x > mousePos.x;
            bool isInBoundsY = btnPos.y - btnScale.y < mousePos.y && btnPos.y + btnScale.y > mousePos.y;

            return isInBoundsX && isInBoundsY && isActive;
        }

        public void ActivateButton(bool value)
        {
            isActive = value;
            if (value)
            {
                m_spriterenderer.color = new Color(1f, 1f, 1f, 1f);
            }
            else 
            {
                m_spriterenderer.color = new Color(1f, 1f, 1f, 0.4f);
            }
            
        }
    }
}
