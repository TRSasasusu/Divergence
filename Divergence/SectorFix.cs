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
                //I still don't know what the underlying problem is, but doing all of this incredibly hacky shit fixes it, so it really doesn't matter anymore.
                Divergence.Instance.ModHelper.Console.WriteLine("guh");

                //Add the player to the cloaking field manually
                Locator.GetCloakFieldController().OnPlayerEnter.Invoke();
                PlayerState._inCloakingField = true;
                GlobalMessenger.FireEvent("EnterCloak");

                //Manually set the renderer fade of the lightbeams to 0 so they actually render
                var _LightSideLightBeam = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_LightSideDockingBay/Effects_LightSideDockingBay/Lightbeam_LightSideDockingBay").GetComponent<OWRenderer>();
                _LightSideLightBeam.SetFade(0);
                var _DarkSideLightBeam = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_DarkSideDockingBay/Effects_DarkSideDockingBay/Lightbeam_DarkSideDockingBay").GetComponent<OWRenderer>();
                _DarkSideLightBeam.SetFade(0);

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
            var _SecretEntranceTrigger = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/SectorTrigger_SecretEntrance").GetComponent<OWTriggerVolume>();
            _SecretEntranceTrigger.AddObjectToVolume(Locator.GetPlayerDetector().gameObject);
            _SecretEntranceTrigger.AddObjectToVolume(Locator.GetPlayerCameraDetector().gameObject);
        }
    }
}

