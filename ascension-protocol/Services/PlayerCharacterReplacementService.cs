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
        private readonly string bundleName;
        private readonly string modelBundlePath;
        private readonly string prefabName;
        private GameObject vroidCharacterPrefab;
        private bool isCharacterReplaced; // キャラクターが置き換えられたかどうか

        public PlayerCharacterReplacementService(string userProfilePath, string modelBundlePath, string bundleName, string prefabName)
        {
            this.userProfilePath = userProfilePath;
            this.modelBundlePath = modelBundlePath;
            this.bundleName = bundleName;
            this.prefabName = prefabName;
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
                var assetBundleLoader = new AssetBundleLoader(userProfilePath, modelBundlePath, bundleName, prefabName);
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

            // モデルのインスタンス化
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

            // ルートオブジェクトをプレイヤーの子オブジェクトに設定
            newPlayerModel.transform.SetParent(player.transform, false);
            newPlayerModel.transform.localPosition = Vector3.zero;
            newPlayerModel.transform.localRotation = Quaternion.identity;
            newPlayerModel.transform.localScale = Vector3.one;

            // レイヤー設定
            Common.Utilities.SetLayerRecursively(newPlayerModel, player.gameObject.layer);

            // ルートオブジェクトを返す
            return newPlayerModel;
        }
    }
}
