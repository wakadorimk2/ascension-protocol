// PlayerPatch.cs
using HarmonyLib;
using UnityEngine;
using Infrastructure.UnityComponents;

namespace Infrastructure.Patches
{
    using System;

    [HarmonyPatch(typeof(EntityPlayerLocal))]
    [HarmonyPatch("Awake")]

    public static class PlayerPatch
    {
        public static void Postfix(EntityPlayerLocal __instance)
        {
            if (__instance == null)

            {
                throw new ArgumentNullException(nameof(__instance));
            }


            Debug.Log("Custom Player Model Loaded");

            // 既存のプレイヤーゲームオブジェクトにPlayerCharacterReplaceBehaviourコンポーネントを追加
            GameObject playerObject = __instance.gameObject;
            var playerCharacterReplace = playerObject.GetComponent<PlayerCharacterReplaceBehaviour>();
            if (playerCharacterReplace == null)


            {
                playerCharacterReplace = playerObject.AddComponent<PlayerCharacterReplaceBehaviour>();
            }

            Debug.Log("Custom Player Model Initialized");
        }
    }
}
