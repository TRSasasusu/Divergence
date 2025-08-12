using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using UnityEngine;

namespace Divergence
{
    [HarmonyPatch]
    internal static class SectorFix
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DreamCampfire), nameof(DreamCampfire.OnExitDreamWorld))]
        private static void DreamCampfire_OnExitDreamWorld(DreamCampfire __instance)
        {
            if (__instance.transform.parent.name.Contains("IP_Dreamfire_Mainframe"))
            {
                //Hack so that artifact lab visuals/audio work properly
                var _LabDarkZone = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/Volumes_SecretEntrance/DarkZone_SecretEntrance").GetComponent<OWTriggerVolume>();
                _LabDarkZone.AddObjectToVolume(Locator.GetPlayerDetector().gameObject);
                _LabDarkZone.AddObjectToVolume(Locator.GetPlayerCameraDetector().gameObject);
                var _LabAudioVolume = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/Volumes_SecretEntrance/AmbienceVolume_Lab").GetComponent<OWTriggerVolume>();
                _LabAudioVolume.AddObjectToVolume(Locator.GetPlayerDetector().gameObject);
                _LabAudioVolume.AddObjectToVolume(Locator.GetPlayerCameraDetector().gameObject);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RingWorldController), nameof(RingWorldController.OnExitDreamWorld))]
        public static void RingWorldController_OnExitDreamWorld_Postfix()
        {
            Locator.GetCloakFieldController().OnPlayerEnter.Invoke();
        }
    }
}

