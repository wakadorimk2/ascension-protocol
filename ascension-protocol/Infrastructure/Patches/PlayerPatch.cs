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

            // プレイヤーゲームオブジェクトを取得
            GameObject playerObject = __instance.gameObject;

            // PlayerCharacterReplaceBehaviourコンポーネントを追加
            var playerCharacterReplace = playerObject.GetComponent<PlayerCharacterReplaceBehaviour>();
            if (playerCharacterReplace == null)
            {
                playerObject.AddComponent<PlayerCharacterReplaceBehaviour>();
            }

            // PlayerAnimationControllerコンポーネントを追加
            var playerAnimationController = playerObject.GetComponent<PlayerAnimationController>();
            if (playerAnimationController == null)
            {
                playerObject.AddComponent<PlayerAnimationController>();
            }

            Debug.Log("Custom Player Model Initialized");
        }
    }
}
