using UnityEngine;


namespace DoomScroll.Common
{
    // Creates and manages custom buttnos
    public class CustomButton
    {
        public GameObject ButtonGameObject { get; private set; }
        private RectTransform m_rectTransform;
        private SpriteRenderer m_spriteRenderer { get; set; }
       // public float CoolDown { get; set; }
        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }

        public Sprite BtnImage { get; set; }
        public Sprite BtnImageHover { get; set; }

        public CustomButton(GameObject parent, Sprite[] images, Vector3 position, Vector2 size, string name)
        {
            ButtonGameObject = new GameObject();
            ButtonGameObject.layer = LayerMask.NameToLayer("UI");
            ButtonGameObject.name = name;
            m_rectTransform = ButtonGameObject.AddComponent<RectTransform>();
            m_rectTransform.SetParent(parent.transform, true);
            m_rectTransform.transform.position = position;
            
            BtnImage = images[0];
            BtnImageHover = images.Length > 1 ? BtnImageHover = images[1] : BtnImageHover = images[0];        
            m_spriteRenderer = ButtonGameObject.AddComponent<SpriteRenderer>();
            m_spriteRenderer.sprite = BtnImage;

            float scale = size.x / m_spriteRenderer.size.x;
            m_spriteRenderer.transform.localScale *= scale;
           
            
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

        public void SetButtonImg(Sprite sprite) 
        {
            m_spriteRenderer.sprite = sprite;
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
            m_rectTransform.transform.localPosition = pos;
        }
    }
}
