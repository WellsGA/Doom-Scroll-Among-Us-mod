using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace DoomScroll
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        //public static SpriteRenderer m_spriterenderer;
        public static CustomButton m_button;
   

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix(HudManager __instance)
        {

            GameObject parent = __instance.gameObject;
            Vector3 mapBtnPos = __instance.MapButton.gameObject.transform.position;
            Vector3 position = new Vector3(mapBtnPos.x, mapBtnPos.y - __instance.MapButton.size.y * 0.7f, mapBtnPos.z);

            Sprite customButtonSprite = ImageLoader.ReadImageFromAssembly(Assembly.GetExecutingAssembly(), "DoomScroll.Assets.cameraFlash.png");
            m_button = new CustomButton(position, parent, customButtonSprite, __instance.MapButton.size * 0.8f);

        }

        // Other patches for the HudManager class

    }
}
