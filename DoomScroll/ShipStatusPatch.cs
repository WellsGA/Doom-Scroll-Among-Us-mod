using HarmonyLib;
using Reactor;

namespace DoomScroll
{
    [HarmonyPatch(typeof(ShipStatus))]
    class ShipStatusPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            ScreenshotManager.Instance.ActivateCameraButton(true);
            Logger<DoomScrollPlugin>.Info("ShipStatusPatch.Start ---- CAMERA ACTIVE");

        }
        /*
        [HarmonyPrefix]
        [HarmonyPatch("StartMeeting")]
        public static void PrefixStartMeeting()
        {
            ScreenshotManager.Instance.ActivateCameraButton(false);
            Logger<DoomScrollPlugin>.Info("ShipStatusPatch.StartMeeting ---- CAMERA INACTIVE");
        }*/
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public static class PatchMinPlayers
    {
        public static void Prefix(GameStartManager __instance)
        {
            __instance.MinPlayers = 2;
        }
    }

}
