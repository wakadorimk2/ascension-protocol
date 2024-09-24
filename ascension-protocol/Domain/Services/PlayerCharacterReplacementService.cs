using System;
using System.Collections;
using UnityEngine;
using Infrastructure.AssetBundles;
using Common;

namespace Domain.Services
{
    public class PlayerCharacterReplacementService
    {
        private readonly string userProfilePath;
        private readonly string assetBundleName;
        private GameObject vroidCharacterPrefab;
        private bool isCharacterReplaced; // キャラクターが置き換えられたかどうか

        public PlayerCharacterReplacementService(string userProfilePath, string assetBundleName)
        {
            this.userProfilePath = userProfilePath;
            this.assetBundleName = assetBundleName;
        }

        public IEnumerator ReplacePlayerCharacter(EntityPlayerLocal player, MonoBehaviour context)
        {
            if (isCharacterReplaced)
            {
                yield break; // 既に置き換え済みなら処理を終了
            }

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Debug.Log("Initializing PlayerCharacterReplacementService");

            if (player == null)
            {
                Debug.LogError("Player is null.");
                yield break;
            }

            // プレハブがnullの場合、アセットバンドルを読み込む
            if (vroidCharacterPrefab == null)
            {
                // アセットバンドルの読み込み
                var assetBundleLoader = new AssetBundleLoader(userProfilePath, assetBundleName);
                yield return context.StartCoroutine(assetBundleLoader.LoadAssetBundleAsync(prefab =>
                {
                    if (prefab != null)
                    {
                        vroidCharacterPrefab = prefab;
                        Debug.Log("vroidCharacterPrefab is set successfully.");
                    }
                    else
                    {
                        Debug.LogError("Prefab is null in callback.");
                    }
                }));
            }
            else
            {
                Debug.Log("VRoidキャラクタープレハブは既にロードされています。");
            }

            if (vroidCharacterPrefab == null)
            {
                Debug.LogError("VRoidキャラクタープレハブの読み込みに失敗しました。");
                yield break;
            }

            // キャラクターの置き換え処理
            HideOriginalPlayerModel(player);

            GameObject newPlayerModel = InstantiateCharacterModel(player);

            if (newPlayerModel != null)
            {
                Debug.Log("VRoidモデルが正常にインスタンス化されました。");
                isCharacterReplaced = true;
            }
            else
            {
                Debug.LogError("VRoidモデルのインスタンス化に失敗しました。");
                yield break;
            }

            // アニメーションの設定
            SetupPlayerAnimations(newPlayerModel, player);

        }

        private static void HideOriginalPlayerModel(EntityPlayerLocal player)
        {
            Transform graphicsTransform = player.transform.Find("Graphics");
            if (graphicsTransform != null)
            {
                graphicsTransform.gameObject.SetActive(false);
                Debug.Log("元のプレイヤーモデルを非表示にしました。");
            }
            else
            {
                Debug.LogWarning("Graphicsオブジェクトが見つかりません。元のモデルを非表示にできませんでした。");
            }
        }

        private GameObject InstantiateCharacterModel(EntityPlayerLocal player)
        {
            Debug.Log("Instantiating VRoid model.");

            // 1. モデルのインスタンス化
            GameObject newPlayerModel = GameObject.Instantiate(vroidCharacterPrefab);

            if (newPlayerModel == null)
            {
                Debug.LogError("Failed to instantiate VRoid model.");
                return null;
            }
            else
            {
                Debug.Log("VRoid model instantiated successfully.");
            }

            // 2. 'Root' 子オブジェクトを取得
            Transform rootTransform = newPlayerModel.transform.Find("Root");
            if (rootTransform == null)
            {
                Debug.LogError("'Root' object not found in the model.");
                return null;
            }

            // 3. 'Root' 子オブジェクトで位置調整を行う
            // 必要に応じて位置、回転、スケールを設定
            Vector3 desiredLocalPosition = new Vector3(0, 0, 0); // 必要な位置オフセット
            rootTransform.localPosition = desiredLocalPosition; // 必要な位置オフセット
            rootTransform.localRotation = Quaternion.identity;
            rootTransform.localScale = Vector3.one;

            // 4. ルートオブジェクトをプレイヤーの子オブジェクトに設定
            newPlayerModel.transform.SetParent(player.transform, false);
            newPlayerModel.transform.localPosition = Vector3.zero;
            newPlayerModel.transform.localRotation = Quaternion.identity;
            newPlayerModel.transform.localScale = Vector3.one;

            // 5. レイヤー設定
            Common.Utilities.SetLayerRecursively(newPlayerModel, player.gameObject.layer);

            // 6. アニメーションの設定
            SetupPlayerAnimations(newPlayerModel, player);

            // 7. ルートオブジェクトを返す
            return newPlayerModel;
        }


        private static void SetupPlayerAnimations(GameObject newPlayerModel, EntityPlayerLocal player)
        {
            Animator playerAnimator = newPlayerModel.GetComponent<Animator>();

            if (playerAnimator == null)
            {
                Debug.LogError("Animator component not found on the new player model's root object.");
                return;
            }

            // 元のプレイヤーの Animator Controller を取得
            Animator originalAnimator = player.GetComponent<Animator>();
            if (originalAnimator == null)
            {
                originalAnimator = player.GetComponentInChildren<Animator>();
            }
            // Animator のパラメータをログ出力
            Common.Utilities.LogAnimatorParameters(originalAnimator);

            // Animator にアタッチされているアニメーションクリップをログ出力
            Common.Utilities.LogAnimatorClips(originalAnimator);

            if (originalAnimator != null)
            {
                // Animator Controller を設定
                playerAnimator.runtimeAnimatorController = originalAnimator.runtimeAnimatorController;
                Debug.Log("Animator Controller を設定しました。");
            }
            else
            {
                Debug.LogError("Original Animator Controller not found on the player.");
            }
        }

    }
}
