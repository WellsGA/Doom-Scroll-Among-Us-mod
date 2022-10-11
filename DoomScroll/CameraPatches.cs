using HarmonyLib;

namespace DoomScroll
{
    [HarmonyPatch(typeof(ShipStatus))]
    class ShipStatusPatch
    {
        /*[HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void PostfixStart()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(true);
        }*/

        [HarmonyPrefix]
        [HarmonyPatch("StartMeeting")]
        public static void PrefixStartMeeting()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(false);
        }
    }

   /* [HarmonyPatch(typeof(GameStartManager))]
    class GameStartManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        public static void PrefixStart()
        {
            ScreenshotManager.Instance.CameraButton.ActivateButton(false);
        }
    }*/
}
