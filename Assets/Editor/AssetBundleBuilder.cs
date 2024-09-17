using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]

    public static void BuildAllAssetBundles()
    {
        // アセットバンドルのビルド先フォルダを指定
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        // アセットバンドルをビルド
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log("アセットバンドルのビルドが完了しました");
    }
}
