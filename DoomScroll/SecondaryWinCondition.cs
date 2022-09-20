using System;
using System.Collections.Generic;
using Reactor;
using HarmonyLib;
using UnityEngine;
using DoomScroll.Common;

namespace DoomScroll
{
    public class SecondaryWinCondition
    {
        private Goal playerSWCGoal;
        private byte playerSWCTarget;
        private bool targetIsVotedOut = false;

        public SecondaryWinCondition()
        {
            assignGoal();
            assignTarget();
        }

        public Goal getPlayerSWCGoal()
        {
            return playerSWCGoal;
        }

        public byte getPlayerSWCTarget()
        {
            return playerSWCTarget;
        }

        public enum Goal
        {
            Protect,
            Frame,
            None
        }

        public void targetWasVotedOut()
        {
            targetIsVotedOut = true;
        }

        public void assignGoal()
        {
            int goalNum = UnityEngine.Random.Range(1, 4); //min inclusive, max exclusive. Will return 1, 2, or 3
            if (goalNum == 1)
            {
                playerSWCGoal = Goal.Protect;
            }
            else if (goalNum == 2)
            {
                playerSWCGoal = Goal.Frame;
            }
            else
            {
                playerSWCGoal = Goal.None;
            }
        }

        public void assignImpostorValues()
        {
            playerSWCGoal = Goal.None;
            playerSWCTarget = byte.MaxValue;
        }

        public void assignTarget()
        {
            int numPlayers = GameData.Instance.AllPlayers.Count;
            int targetNum = UnityEngine.Random.Range(0, numPlayers - 1);
            for (int i = 0; i < numPlayers; i++)
            {
                if (i == targetNum)
                {
                    playerSWCTarget = GameData.Instance.AllPlayers[i].PlayerId;
                    break;
                }
            }

        }

        public bool CheckSuccess()
        {
            int numPlayers = GameData.Instance.AllPlayers.Count;
            if (playerSWCGoal == Goal.None)
            {
                return true;
            }
            else if (playerSWCGoal == Goal.Protect)
            {
                for (int i = 0; i < numPlayers; i++)
                {
                    if (playerSWCTarget == GameData.Instance.AllPlayers[i].PlayerId)
                    {
                        if (GameData.Instance.AllPlayers[i].IsDead) //player is dead
                        {
                            return false;
                        }
                        else //all other cases, i.e. player is not dead
                        {
                            return true;
                        }
                    }
                }
            }
            else if (playerSWCGoal == Goal.Frame)
            {
                if (targetIsVotedOut)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //playerSWCGoal is null
            return false;
        }

        public string SWCSuccessMessage()
        {
            bool wasSuccessful = CheckSuccess();
            if (playerSWCGoal == Goal.None)
            {
                return "N/A";
            }
            else if (wasSuccessful)
            {
                return "Success";
            }
            else if (!wasSuccessful)
            {
                return "Failure";
            }
            else
            {
                return "";
            }
        }
    }
}
