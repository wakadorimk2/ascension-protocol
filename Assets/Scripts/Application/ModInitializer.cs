using HarmonyLib;
using UnityEngine;

namespace AscensionProtocol.Application
{
    /// <summary>
    /// Modの初期化を行うクラス.
    /// </summary>
    public class ModInitializer : IModApi
    {
        private Harmony harmonyInstance;

        /// <summary>
        /// Modを初期化するメソッド.
        /// </summary>
        /// <param name="_modInstance">Modインスタンス.</param>
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
                throw; // 例外を再スロー
            }
        }

        // Modが終了する際に呼ばれるメソッドがある場合、そこでDisposeを行います。
        // 例として、以下のようなメソッドを追加します。
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
