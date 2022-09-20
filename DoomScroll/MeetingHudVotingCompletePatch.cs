using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
    public static class MeetingHudVotingCompletePatch
    {
        public static void Prefix(MeetingHud __instance, ref GameData.PlayerInfo exiled)
        {
            if (exiled != null)
            {
                SecondaryWinConditionHolder.checkTargetVotedOut(exiled);
            }
        }
    }
}