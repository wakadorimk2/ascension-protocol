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
            Debug.Log("Instantiate VRoid model");

            Transform playerTransform = player.transform;

            GameObject newPlayerModel = GameObject.Instantiate(vroidCharacterPrefab, playerTransform.position, playerTransform.rotation);
            
            if (newPlayerModel == null)
            {
                Debug.LogError("Failed to instantiate VRoid model.");
                return null;
            }
            else
            {
                Debug.Log("VRoid model instantiated successfully");
            }

            // 'Root'オブジェクトを取得
            Transform rootTransform = newPlayerModel.transform.Find("Root");
            if (rootTransform == null)
            {
                Debug.LogError("'root' object not found in the model.");
                return null;
            }

            rootTransform.transform.SetParent(playerTransform, false);

            // レイヤー設定
            Utilities.SetLayerRecursively(newPlayerModel, player.gameObject.layer);
            rootTransform.transform.localRotation = Quaternion.identity;
            rootTransform.transform.localScale = Vector3.one;

            return rootTransform.gameObject;
        }

        private void SetupPlayerAnimations(GameObject newPlayerModel, EntityPlayerLocal player)
        {
            Animator playerAnimator = newPlayerModel.GetComponent<Animator>();
            if (playerAnimator == null)
            {
                playerAnimator = newPlayerModel.AddComponent<Animator>();
                Debug.Log("Animatorコンポーネントを追加しました。");
            }

            Animator vroidAnimator = vroidCharacterPrefab.GetComponent<Animator>();
            if (vroidAnimator != null)
            {
                playerAnimator.avatar = vroidAnimator.avatar;
                Debug.Log("VRoidモデルのアバターを設定しました。");
            }
            else
            {
                Debug.LogError("VRoidモデルにAnimatorが存在しません。");
            }

            Animator originalAnimator = player.GetComponentInChildren<Animator>();
            if (originalAnimator != null)
            {
                playerAnimator.runtimeAnimatorController = originalAnimator.runtimeAnimatorController;
                Debug.Log("Animator Controllerを設定しました。");
            }
            else
            {
                Debug.LogWarning("元のプレイヤーのAnimatorが見つかりません。");
            }
        }
    }
}
