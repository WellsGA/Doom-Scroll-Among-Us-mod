using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Reactor;
using System.Reflection;


namespace DoomScroll
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        public static event Action m_listener;
        private static CustomButton m_buttonGo;

        internal static void InitHudManager()
        {
            m_listener += OnClickCamera;
            Logger<DoomScrollPlugin>.Info("init hud manager called");

        }
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix(HudManager __instance)
        {
            GameObject parent = __instance.gameObject;
            Vector3 mapBtnPos = __instance.MapButton.gameObject.transform.position;
            Vector3 position = new Vector3(mapBtnPos.x, mapBtnPos.y - __instance.MapButton.size.y * __instance.MapButton.transform.localScale.y, mapBtnPos.z);

            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png");
            m_buttonGo = new CustomButton(position, parent, customButtonSprite, __instance.MapButton.size * __instance.MapButton.transform.localScale);
           // sadly this doesn't work... // m_buttonGo.m_Button.onClick.AddListener(m_listener);

        }


        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void Postfix()
        {
            try
            {
               
                if (IsClicked() && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_listener?.Invoke();
                }
               
            }
            catch
            {
                Logger<DoomScrollPlugin>.Error("Couldn't invoke delegate methods.");
            }
        }

        static void OnClickCamera()
        {
            // To DO: Implement onClick behavior
            Logger<DoomScrollPlugin>.Info("CAMERA BUTTON PRESSED");
        }

        static bool IsClicked() 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // bool isOnSprite = m_buttonGo.m_Spriterenderer.bounds.Contains(mousePos);

            Vector3 itemPos = m_buttonGo.m_Spriterenderer.transform.position;
             Vector3 itemScale = m_buttonGo.m_Spriterenderer.bounds.extents;

             bool isInBoundsX = itemPos.x - itemScale.x  < mousePos.x && itemPos.x + itemScale.x > mousePos.x;
             bool isInBoundsY = itemPos.y - itemScale.y < mousePos.y && itemPos.y + itemScale.y > mousePos.y;

            if (isInBoundsX && isInBoundsY)
            {
                m_buttonGo.m_Spriterenderer.color = Color.cyan;
            } else
            {
                m_buttonGo.m_Spriterenderer.color = Color.white;
            }

            // return isOnSprite && Input.GetKeyUp(KeyCode.Mouse0);
            return isInBoundsX && isInBoundsY; 
           
        }
    }
}
