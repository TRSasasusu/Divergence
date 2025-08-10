using HarmonyLib;
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
            Divergence.Instance.ModHelper.Console.WriteLine("Has exited the dreamworld.");
            if (__instance.transform.parent.name.Contains("IP_Dreamfire_Mainframe"))
            {
                Divergence.Instance.ModHelper.Console.WriteLine("Fixing Dreamfire sector stuff...");
                var _SecretEntranceSector = GameObject.Find("RingWorld_Body/Sector_RingWorld/SectorTrigger_RingWorld").GetComponent<OWTriggerVolume>();
                _SecretEntranceSector.AddObjectToVolume(Locator.GetPlayerDetector().gameObject);
            }
        }
    }
}

