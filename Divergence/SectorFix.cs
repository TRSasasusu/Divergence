using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Collections.Generic;
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
                //Add the player to the cloaking field manually
                Locator.GetCloakFieldController().OnPlayerEnter.Invoke();

                //Turning RingInteriorSectorTriggerVolume off and back on again is literally the only thing that fixes this, so that's what we're doing I guess
                var _RingWorldSector = GameObject.Find("RingWorld_Body/Sector_RingWorld/Volumes_RingWorld/RingInteriorSectorTriggerVolume");
                _RingWorldSector.SetActive(false);
                _RingWorldSector.SetActive(true);

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
        [HarmonyPatch(typeof(DreamWorldController), nameof(DreamWorldController.ExitDreamWorld), [typeof(DreamWakeType)])]
        public static void DreamWorldController_ExitDreamWorld()
        {
            var _SecretEntranceSector = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/SectorTrigger_SecretEntrance").GetComponent<OWTriggerVolume>();
            _SecretEntranceSector.AddObjectToVolume(Locator.GetPlayerDetector().gameObject);
            _SecretEntranceSector.AddObjectToVolume(Locator.GetPlayerCameraDetector().gameObject);
        }
    }
}

