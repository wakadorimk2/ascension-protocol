using UnityEngine;
using System.Collections;
using Domain.Services;

namespace Infrastructure.UnityComponents
{
    public class PlayerCharacterReplaceBehaviour : MonoBehaviour
    {
        private PlayerCharacterReplacementService replacementService;
        private bool isInitialized;

        void Awake()
        {
            Debug.Log("PlayerCharacterReplaceBehaviour Awake called.");
            string userProfilePath = "C:/Users/wakad/AppData/Roaming/7DaysToDie/Mods/ascension-protocol";
            string modelBundlePath = "Bundles";
            string bundleName = "models.bundle";
            string prefabName = "pink_twin";
            replacementService = new PlayerCharacterReplacementService(userProfilePath, modelBundlePath, bundleName, prefabName);
            StartCoroutine(InitializeWhenPlayerReady());
        }

        private IEnumerator InitializeWhenPlayerReady()
        {
            Debug.Log("InitializeWhenPlayerReady called.");

            EntityPlayerLocal player = null;

            // プレイヤーが完全にロードされるまで待機
            while (!isInitialized)
            {
                player = GameManager.Instance.World?.GetPrimaryPlayer();
                if (player != null && player.IsAlive())
                {
                    isInitialized = true;
                    yield return StartCoroutine(replacementService.ReplacePlayerCharacter(player, this));
                }
                else
                {
                    yield return null;
                }
            }

            // プレイヤーが完全にロードされたら処理を続行

            // PlayerAnimationControllerBehaviourコンポーネントを追加
            Debug.Log("Add PlayerAnimationControllerBehaviour to player.");
            var playerAnimationController = player.gameObject.GetComponent<PlayerAnimationControllerBehaviour>();
            if (playerAnimationController == null)
            {
                player.gameObject.AddComponent<PlayerAnimationControllerBehaviour>();
            }
            else
            {
                Debug.Log("PlayerAnimationControllerBehaviour already exists.");
            }

            Debug.Log("InitializeWhenPlayerReady finished.");
        }
    }
}
