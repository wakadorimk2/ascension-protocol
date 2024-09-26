using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace Infrastructure.AssetBundles
{
    public class AssetBundleLoader
    {

        private readonly string userProfilePath;
        private readonly string bundleName;
        private readonly string modelBundlePath;
        private readonly string prefabName;
        private static AssetBundle loadedAssetBundle;

        public AssetBundleLoader(string userProfilePath, string modelBundlePath, string bundleName, string prefabName)
        {
            this.userProfilePath = userProfilePath;
            this.modelBundlePath = modelBundlePath;
            this.bundleName = bundleName;
            this.prefabName = prefabName;
        }

        public IEnumerator LoadAssetBundleAsync(System.Action<GameObject> onComplete)
        {
            if (onComplete == null)
            {
                throw new ArgumentNullException(nameof(onComplete));
            }

            string assetBundlePath = Path.Combine(userProfilePath, modelBundlePath, bundleName);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            yield return request;

            loadedAssetBundle = request.assetBundle;
            if (loadedAssetBundle != null)
            {
                AssetBundleRequest prefabRequest = loadedAssetBundle.LoadAssetAsync<GameObject>(prefabName);
                yield return prefabRequest;

                GameObject prefab = prefabRequest.asset as GameObject;
                if (prefab != null)
                {
                    Debug.Log("VRoidキャラクタープレハブの読み込みに成功しました。");
                    onComplete(prefab);
                }
                else
                {
                    Debug.LogError("プレハブがバンドルに存在しません。");
                    onComplete(null);
                }
            }
            else
            {
                Debug.LogError("アセットバンドルのロードに失敗しました。パスを確認してください。");
                onComplete(null);
            }
        }
    }
}
