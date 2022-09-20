using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class RoleManagerSelectRolesPatch
    {
        public static void Postfix(RoleManager __instance)
        {
            SecondaryWinConditionHolder.clearPlayerSWCList(); // ensures list is empty before filling it

            foreach (GameData.PlayerInfo player in GameData.Instance.AllPlayers)
            {
                if (player.Role.IsImpostor)
                {
                    PlayerSWCTracker secWinCondImpostor = new PlayerSWCTracker(player.PlayerId);
                    secWinCondImpostor.impostorSWC();
                    SecondaryWinConditionHolder.addToPlayerSWCList(secWinCondImpostor);
                    Logger<DoomScrollPlugin>.Message("" + secWinCondImpostor.getPlayerID() + ", SWC: " + secWinCondImpostor.getSWC());
                }
                else if (!player.Role.IsImpostor)
                {
                    PlayerSWCTracker secWinCondCrewmate = new PlayerSWCTracker(player.PlayerId);
                    SecondaryWinConditionHolder.addToPlayerSWCList(secWinCondCrewmate);
                    Logger<DoomScrollPlugin>.Message("PID: " + secWinCondCrewmate.getPlayerID() + ", SWC: " + secWinCondCrewmate.getSWC());
                }
            }

        }
    }
}
