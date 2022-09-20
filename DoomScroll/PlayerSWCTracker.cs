using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    public class PlayerSWCTracker
    {
        private byte PlayerID;
        private SecondaryWinCondition SWC;

        public PlayerSWCTracker(byte pID)
        {
            PlayerID = pID;
            SWC = new SecondaryWinCondition();
        }

        public void impostorSWC()
        {
            SWC.assignImpostorValues();
        }

        public byte getPlayerID()
        {
            return PlayerID;
        }

        public SecondaryWinCondition getSWC()
        {
            return SWC;
        }
    }
}
