// GameManagerStartGamePatch.cs
using HarmonyLib;
using UnityEngine;
using System.Collections;
using Infrastructure.UnityComponents;

namespace Infrastructure.Patches
{
    using System;

    [HarmonyPatch(typeof(GameManager))]
    [HarmonyPatch("StartGame")]
    public static class GameManagerStartGamePatch
    {
        public static void Postfix(GameManager __instance)
        {
            if (__instance == null)
            {
                throw new ArgumentNullException(nameof(__instance));
            }


            Debug.Log("World has been loaded.");


            // プレイヤーキャラクター置き換えの初期化処理を開始
            __instance.StartCoroutine(InitializePlayerCharacterReplace());
        }


        private static IEnumerator InitializePlayerCharacterReplace()
        {
            // プレイヤーが完全にロードされるまで待機
            while (GameManager.Instance.World == null || GameManager.Instance.World.GetPrimaryPlayer() == null)
            {
                yield return null;
            }

            EntityPlayerLocal player = GameManager.Instance.World.GetPrimaryPlayer();

            if (player != null)
            {
                var playerCharacterReplace = player.gameObject.GetComponent<PlayerCharacterReplaceBehaviour>();
                if (playerCharacterReplace == null)
                {
                    playerCharacterReplace = player.gameObject.AddComponent<PlayerCharacterReplaceBehaviour>();
                    Debug.Log("PlayerCharacterReplaceBehaviour component added to player.");
                }
            }
            else
            {
                Debug.LogError("Player not found.");
            }
        }
    }
}
