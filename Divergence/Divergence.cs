using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using UnityEngine;

namespace Divergence
{
    public class Divergence : ModBehaviour
    {
        public static Divergence Instance;
        public INewHorizons NewHorizons;

        public void Awake()
        {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        public void Start()
        {
            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);
            ModHelper.Console.WriteLine($"{nameof(Divergence)} is loaded!", MessageType.Success);
            Patch.Initialize();
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        private void OnCompleteSceneLoad(OWScene scene, OWScene loadScene)
        {
            if (loadScene != OWScene.SolarSystem) return;

            var DreamWorldAudioController = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/AudioController_DreamWorld");
            DreamWorldAudioController.AddComponent<MainframeAudioHandler>();

            //Super ugly fix for sector cull group pop-in when warping from the dreamfire
            var _SecretEntranceStructures = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/Structures_SecretEntrance").GetComponent<SectorCullGroup>();
            var _SecretEntranceProps = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/Props_SecretEntrance").GetComponent<SectorCullGroup>();
            var _SecretEntranceInteractables = GameObject.Find("RingWorld_Body/Sector_RingWorld/Sector_SecretEntrance/Interactibles_SecretEntrance").GetComponent<SectorCullGroup>();
            _SecretEntranceStructures._crossfadeLength = 0;
            _SecretEntranceProps._crossfadeLength = 0;
            _SecretEntranceInteractables._crossfadeLength = 0.1f;
        }
    }

}
