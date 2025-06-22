using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;

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
            ModHelper.Console.WriteLine($"My mod {nameof(Divergence)} is loaded!", MessageType.Success);
            Patch.Initialize();
        }
    }

}
