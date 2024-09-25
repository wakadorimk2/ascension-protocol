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
        }

        void Start()
        {
            Debug.Log("PlayerCharacterReplaceBehaviour Start called.");
            StartCoroutine(InitializeWhenPlayerReady());
        }

        private IEnumerator InitializeWhenPlayerReady()
        {
            // プレイヤーが完全にロードされるまで待機
            while (!isInitialized)
            {
                EntityPlayerLocal player = GameManager.Instance.World?.GetPrimaryPlayer();
                if (player != null && player.IsAlive())
                {
                    isInitialized = true;
                    yield return StartCoroutine(replacementService.ReplacePlayerCharacter(player, this));
                }
                else
                {
                    yield return null; // 次のフレームまで待機
                }
            }
        }
    }
}
