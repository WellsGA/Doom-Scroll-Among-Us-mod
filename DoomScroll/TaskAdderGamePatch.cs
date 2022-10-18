using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(TaskAdderGame))]
    class TaskAdderGamePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("ShowFolder")]
        public static void PostfixShowFolder(TaskAdderGame __instance, TaskAddButton taskAddButton)
        {
            taskAddButton.Text.text = "SWC: " + SecondaryWinConditionHolder.getSomePlayerSWC(PlayerControl.LocalPlayer._cachedData.PlayerId).SWCAssignText();
        }
    }
}
