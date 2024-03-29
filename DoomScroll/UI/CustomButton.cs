﻿using UnityEngine;
using DoomScroll.Common;
using Reactor;


namespace DoomScroll.UI
{
    public enum ImageType{
        DEFAULT,
        HOVER
    }
    // Creates and manages custom buttnos
    public class CustomButton
    {
        public DoomScrollEvent ButtonEvent = new DoomScrollEvent();

        public GameObject ButtonGameObject { get; private set; }
        // private BoxCollider2D m_collider;
        private SpriteRenderer m_spriteRenderer;
        private Sprite[] buttonIcons;
        private bool isDefaultImg;
        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }
       

        public CustomButton(GameObject parent, Sprite[] images, Vector3 position, float scaledX, string name)
        {
            ButtonGameObject = new GameObject(name);
            ButtonGameObject.layer = LayerMask.NameToLayer("UI");
            SetParent(parent);
            SetLocalPosition(position);
            buttonIcons = images;
            m_spriteRenderer = ButtonGameObject.AddComponent<SpriteRenderer>();
            m_spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            SetButtonImg(ImageType.DEFAULT);
            // size has to be set after setting the image!
            //make sure the images are sized correctly and scaled proportionately 
            ScaleSize(scaledX);
            ActivateButton(true);
            EnableButton(true);
        }

        public CustomButton( GameObject parent, Sprite[] images, string name)
        {
            ButtonGameObject = new GameObject(name);
            ButtonGameObject.layer = LayerMask.NameToLayer("UI");
            SetParent(parent);
            buttonIcons = images;
            m_spriteRenderer = ButtonGameObject.AddComponent<SpriteRenderer>();
            m_spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            SetButtonImg(ImageType.DEFAULT);
            // size has to be set after setting the image!

            ActivateButton(true);
            EnableButton(true);
        }

        public void SetLocalPosition(Vector3 pos)
        {
            ButtonGameObject.transform.localPosition = pos;
        }

        public void ScaleSize(float scaledWidth) 
        {
            m_spriteRenderer.size = new Vector2(scaledWidth, m_spriteRenderer.sprite.rect.height * scaledWidth / m_spriteRenderer.sprite.rect.width);

        }
        public void SetParent(GameObject parent)
        {
            ButtonGameObject.transform.SetParent(parent.transform, true);
        }

        public bool isHovered()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 btnPos = m_spriteRenderer.transform.position;
            Vector3 btnScale = m_spriteRenderer.bounds.extents;

            bool isInBoundsX = btnPos.x - btnScale.x < mousePos.x && btnPos.x + btnScale.x > mousePos.x;
            bool isInBoundsY = btnPos.y - btnScale.y < mousePos.y && btnPos.y + btnScale.y > mousePos.y;

            return isInBoundsX && isInBoundsY && IsEnabled && IsActive;
        }

        private void SetButtonImg(ImageType type)
        {
            switch (type) {
                case ImageType.DEFAULT:
                    m_spriteRenderer.sprite = buttonIcons[0];
                    isDefaultImg = true;
                    break;
                case ImageType.HOVER:
                    m_spriteRenderer.sprite = buttonIcons.Length > 1 ? buttonIcons[1] : buttonIcons[0];
                    isDefaultImg = false;
                    break;
                default:
                    m_spriteRenderer.sprite = buttonIcons[0];
                    isDefaultImg = true;
                    break;
            }              
        }

        public void ReplaceImgageOnHover() 
        {
            if (isDefaultImg && isHovered())
            {
                SetButtonImg(ImageType.HOVER);
            }
            else if (!isDefaultImg && !isHovered())
            {
                SetButtonImg(ImageType.DEFAULT);
            }
        }
        public void EnableButton(bool value)
        {
            IsEnabled = value;
            if (value)
            {
                m_spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
            else 
            {
                m_spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
            }  
        }

        public void ActivateButton(bool value)
        {
            IsActive = value;
            ButtonGameObject.SetActive(value);   
        }     
    }
}
