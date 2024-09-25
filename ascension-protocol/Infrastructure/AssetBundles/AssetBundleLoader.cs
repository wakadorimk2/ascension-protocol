// AssetBundleLoader.cs
using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace Infrastructure.AssetBundles
{
    public class AssetBundleLoader
    {

        private readonly string userProfilePath;
        private readonly string assetBundleName;
        private static AssetBundle loadedAssetBundle;

        public AssetBundleLoader(string userProfilePath, string assetBundleName)


        {
            this.userProfilePath = userProfilePath;
            this.assetBundleName = assetBundleName;
        }

        public IEnumerator LoadAssetBundleAsync(System.Action<GameObject> onComplete)
        {
            if (onComplete == null)
            {
                throw new ArgumentNullException(nameof(onComplete));
            }

            string assetBundlePath = Path.Combine(userProfilePath, assetBundleName);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            yield return request;

            loadedAssetBundle = request.assetBundle;
            if (loadedAssetBundle != null)
            {
                string prefabName = Path.GetFileNameWithoutExtension(assetBundleName);
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
