using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    public static class SecondaryWinConditionHolder
    {
        private static List<PlayerSWCTracker> playerSWCList = new List<PlayerSWCTracker>();

        public static void addToPlayerSWCList(PlayerSWCTracker playerSWCInfo)
        {
            playerSWCList.Add(playerSWCInfo);
        }

        public static void clearPlayerSWCList()
        {
            playerSWCList = new List<PlayerSWCTracker>();
        }

        public static void checkTargetVotedOut(GameData.PlayerInfo votedOutPlayer)
        {
            foreach (PlayerSWCTracker playerSWC in playerSWCList)
            {
                byte playerSWCTargetID = playerSWC.getSWC().getPlayerSWCTarget();
                if (votedOutPlayer.PlayerId == playerSWCTargetID)
                {
                    playerSWC.getSWC().targetWasVotedOut();
                }
            }
        }
    }
}
