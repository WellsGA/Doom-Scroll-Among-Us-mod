using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(RoleManager))]
    public static class RoleManagerPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("SelectRoles")]
        public static void PostfixSelectRoles(RoleManager __instance)
        {
            Logger<DoomScrollPlugin>.Info("Select Roles Patch is running!!\n There should be Secondary Win Conditions below:\n");
            SecondaryWinConditionHolder.clearPlayerSWCList(); // ensures list is empty before filling it

            foreach (GameData.PlayerInfo player in GameData.Instance.AllPlayers)
            {
                if (player.Role.IsImpostor)
                {
                    PlayerSWCTracker secWinCondImpostor = new PlayerSWCTracker(player.PlayerId);
                    secWinCondImpostor.impostorSWC();
                    SecondaryWinConditionHolder.addToPlayerSWCList(secWinCondImpostor);
                    Logger<DoomScrollPlugin>.Info("" + secWinCondImpostor.getPlayerID() + ", SWC: " + secWinCondImpostor.getSWC());
                }
                else if (!player.Role.IsImpostor)
                {
                    PlayerSWCTracker secWinCondCrewmate = new PlayerSWCTracker(player.PlayerId);
                    SecondaryWinConditionHolder.addToPlayerSWCList(secWinCondCrewmate);
                    Logger<DoomScrollPlugin>.Info("PID: " + secWinCondCrewmate.getPlayerID() + ", SWC: " + secWinCondCrewmate.getSWC());
                }
            }

        }
    }
}
