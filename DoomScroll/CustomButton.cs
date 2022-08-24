using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Reactor;

namespace DoomScroll
{
    // Creates and manages custom buttnos
    public class CustomButton
    {
        private GameObject m_button;
        private SpriteRenderer m_spriterenderer;

        public CustomButton(Vector3 position, GameObject parent, Sprite buttonImg, Vector2 size)
        {
            m_button = new GameObject();
            m_button.transform.SetParent(parent.transform);
            m_button.transform.localPosition = position;

            m_spriterenderer = m_button.AddComponent<SpriteRenderer>();
            m_spriterenderer.sprite = buttonImg;
           
            float scale = size.x / m_spriterenderer.size.x;
            m_spriterenderer.transform.localScale *= scale;

            // debug: Logger<DoomScrollPlugin>.Info(" sprite renderer size: " + m_spriterenderer.size);

            m_button.SetActive(true);

            /* For some reason this doesn't work ... remove it later
             * DefaultControls.Resources uiElement = new DefaultControls.Resources();
             //sets the button background to buttonImg
             uiElement.standard = buttonImg;
             m_button = DefaultControls.CreateButton(uiElement);
             m_button.transform.SetParent(parent.transform, false);*/
        }

        // To do: implemet button behavior  -- active, disabled, onClick, etc.

    }
}
