﻿using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace DoomScroll
{
    //Disable chat scrolling limit:
    [HarmonyPatch(typeof(ObjectPoolBehavior), nameof(ObjectPoolBehavior.Reclaim))]
    public static class ObjectPoolBehaviorReclaimPatch
    {
        public static void Postfix(ObjectPoolBehavior __instance, PoolableBehavior obj)
        {
            Il2CppSystem.Collections.Generic.List<PoolableBehavior> obj2 = __instance.activeChildren;
            lock (obj2)
            {
                if (__instance.inactiveChildren.Remove(obj))
                {
                    __instance.activeChildren.Insert(0, obj);
                }
                else if (__instance.activeChildren.Contains(obj))
                {
                    Debug.Log("ObjectPoolBehavior: :| Something was reclaimed without being gotten");
                }
                else
                {
                    Debug.Log("ObjectPoolBehavior: Destroying this thing I don't own");
                    UnityEngine.Object.Destroy(obj.gameObject);
                }
            }
        }
    }
}
