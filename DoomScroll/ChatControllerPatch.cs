using System;
using System.Collections.Generic;
using HarmonyLib;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(ChatController) ) ]
    static class ChatControllerPatch
    {
        private static DoomScrollEvent FolderToggle = new DoomScrollEvent();

        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        public static void PostfixStart()
        {
            // subscribe methods to call on buttonclick
            FolderToggle.MyAction += FolderManager.Instance.OnClickFolderBtn;
        }
        //make update and toggle patches
    }
}
