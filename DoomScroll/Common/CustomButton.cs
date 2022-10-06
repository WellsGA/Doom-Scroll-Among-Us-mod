using UnityEngine;
using Reactor;


namespace DoomScroll.Common
{
    public enum ImageType{
        DEFAULT,
        HOVER
    }
    // Creates and manages custom buttnos
    public class CustomButton
    {
        public GameObject ButtonGameObject { get; private set; }
        private BoxCollider2D m_collider;
        private SpriteRenderer m_spriteRenderer;
        private Sprite[] buttonIcons;
        private bool isDefaultImg;
        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }
       

        public CustomButton(GameObject parent, Sprite[] images, Vector3 position, Vector2 scaledsize, string name)
        {
            ButtonGameObject = new GameObject();
            ButtonGameObject.layer = LayerMask.NameToLayer("UI");
            ButtonGameObject.name = name;
            ButtonGameObject.transform.SetParent(parent.transform, true);
            ButtonGameObject.transform.localPosition = position;
            m_collider = ButtonGameObject.AddComponent<BoxCollider2D>();
            
            buttonIcons = images;
            m_spriteRenderer = ButtonGameObject.AddComponent<SpriteRenderer>();
            m_spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            SetButtonImg(ImageType.DEFAULT);
            // size has to be set after setting the image!
            m_collider.size = scaledsize;
            m_spriteRenderer.size = scaledsize;

            ActivateButton(true);
            EnableButton(true);
            // debug: Logger<DoomScrollPlugin>.Info(" sprite renderer size: " + m_spriterenderer.size);
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

        public void SetButtonImg(ImageType type)
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
                Logger<DoomScrollPlugin>.Info("BUTTON HOVER");
            }
            else if (!isDefaultImg && !isHovered())
            {
                SetButtonImg(ImageType.DEFAULT);
                Logger<DoomScrollPlugin>.Info("BUTTON NOT HOVER");
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

        public void SetLocalPosition(Vector3 pos) 
        {
            ButtonGameObject.transform.localPosition = pos;
        }
    }
}
