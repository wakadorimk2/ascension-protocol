using HarmonyLib;
using UnityEngine;

namespace AscensionProtocol.Application
{
    public class ModInitializer : IModApi
    {
        private Harmony harmonyInstance;
        public void InitMod(Mod _modInstance)
        {
            Debug.Log("Initializing Mod API");

            try
            {
                harmonyInstance = new Harmony("com.ascension-protocol.mod");
                harmonyInstance.PatchAll();

                Debug.Log("Harmony Patching Successful");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Harmony Patching Failed: {ex}");
                throw;
            }
        }
        public void ShutdownMod()
        {
            if (harmonyInstance != null)
            {
                harmonyInstance.UnpatchSelf();
                Debug.Log("Harmony Patches Unapplied");
                harmonyInstance = null;
            }
        }
    }
}
