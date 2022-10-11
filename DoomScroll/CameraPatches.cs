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
            ScreenshotManager.Instance.CameraButton.ActivateButton(true);
            Logger<DoomScrollPlugin>.Info("ShipStatusPatch Start ---- CAMERA ACTIVE");

        }

        [HarmonyPrefix]
        [HarmonyPatch("StartMeeting")]
        public static void PrefixStartMeeting()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(false);
            Logger<DoomScrollPlugin>.Info("ShipStatusPatch StartMeeting ---- CAMERA INACTIVE");
        }
    }

    /*[HarmonyPatch(typeof(GameStartManager))]
    class GameStartManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        public static void PrefixStart()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(false);
            Logger<DoomScrollPlugin>.Info("GameStartManager Start ---- CAMERA ACTIVE");
        }
    }*/
}
