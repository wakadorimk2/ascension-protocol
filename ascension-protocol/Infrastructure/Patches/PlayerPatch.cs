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

            // Replaceが完了したらPlayerAnimationControllerBehaviourコンポーネントを追加
            if (playerCharacterReplace != null)
            {
                var playerAnimationController = playerObject.GetComponent<PlayerAnimationControllerBehaviour>();
                if (playerAnimationController == null)
                {
                    playerObject.AddComponent<PlayerAnimationControllerBehaviour>();
                }
            }

            Debug.Log("Custom Player Model Initialized");
        }
    }
}
