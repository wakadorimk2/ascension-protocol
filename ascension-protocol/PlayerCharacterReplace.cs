using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class MyModApi : IModApi
{
    public void InitMod(Mod _modInstance)
    {
        Debug.Log("Initializing Mod API");

        try
        {
            var harmony = new Harmony("com.ascension-protocol.mod");
            harmony.PatchAll();
            Debug.Log("Harmony Patching Successful");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Harmony Patching Failed: " + ex.Message);
        }
    }
}

public class PlayerCharacterReplace : MonoBehaviour
{
    public string UserProfilePath = "C:/Users/wakad/AppData/Roaming/7DaysToDie/Mods/AscensionProtocol";
    public string assetBundleName = "pink_twin.unity3d"; // AssetBundleのパスを指定

    // VRoidモデルのPrefab（インポートしたキャラクター）
    public GameObject vroidCharacterPrefab;

    // プレイヤーの元のモデル（置き換え対象）
    private GameObject originalPlayerModel;

    // プレイヤーキャラクターの位置を保持するための参照
    private Transform playerTransform;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // プレイヤーモデルがシーンにロードされるのを待つ
        yield return new WaitForSeconds(0.5f); // 0.5秒待機

        // プレイヤーの元のモデルを取得
        EntityPlayer player = FindObjectOfType<EntityPlayer>();  // EntityPlayerコンポーネントを持つオブジェクトを検索
        if (player != null)
        {
            originalPlayerModel = player.gameObject;
        }

        if (originalPlayerModel != null)
        {
            // プレハブを読み込む
            vroidCharacterPrefab = LoadCharacterFromAssetBundle();

            if (vroidCharacterPrefab == null)
            {
                Debug.LogError("VRoidキャラクタープレハブの読み込みに失敗しました。");
                yield break;
            }
            else
            {
                Debug.Log("VRoidキャラクタープレハブの読み込みに成功しました。");

                // プレイヤーのTransformを取得（位置・回転・スケールを保持する）
                playerTransform = originalPlayerModel.transform;

                // 元のプレイヤーモデルを非表示または削除
                originalPlayerModel.SetActive(false);

                GameObject newPlayerModel = Instantiate(vroidCharacterPrefab, playerTransform.position, playerTransform.rotation);

                // デバッグメッセージでモデルの位置などを確認
                Debug.Log($"VRoidモデルを生成しました。位置: {playerTransform.position}, 回転: {playerTransform.rotation}");

                // 生成されたVRoidキャラクターをプレイヤーに追従させる
                newPlayerModel.transform.SetParent(playerTransform);

                // アニメーションやリグの設定をプレイヤーに対応させる
                SetupPlayerAnimations(newPlayerModel);

            }
        }
        else
        {
            Debug.LogError("元のプレイヤーモデルが見つかりません。タグを確認してください。");
        }
    }

    // プレイヤーにアニメーションを設定する関数
    void SetupPlayerAnimations(GameObject newPlayerModel)
    {
        Animator playerAnimator = newPlayerModel.GetComponent<Animator>();

        if (playerAnimator != null)
        {
            // アニメーションコントローラーを設定
            playerAnimator.runtimeAnimatorController = Resources.Load("PlayerAnimatorController") as RuntimeAnimatorController;
        }
        else
        {
            Debug.LogError("VRoidモデルにAnimatorがありません。リグ設定を確認してください。");
        }
    }

    public GameObject LoadCharacterFromAssetBundle()
    {
        // 自分の位置とディレクトリを表示
        string currentDirectory = Directory.GetCurrentDirectory();
        Debug.LogFormat("Current Directory: {0}", currentDirectory);

        string assetBundlePath = Path.Combine(UserProfilePath, assetBundleName);
        AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath);

        if (bundle != null)
        {
            GameObject prefab = bundle.LoadAsset<GameObject>("ピンクのツインテちゃん"); // プレハブ名を指定

            if (prefab != null)
            {
                bundle.Unload(false);
                return prefab;
            }
            else
            {
                Debug.LogError("プレハブがバンドルに存在しません。");
                return null;
            }
        }
        else
        {
            Debug.LogError("アセットバンドルのロードに失敗しました。パスを確認してください。");
            return null;
        }
    }

    [HarmonyPatch(typeof(EntityPlayer))]  // Playerクラスにパッチを適用
    [HarmonyPatch("Start")]  // プレイヤーの開始処理にパッチを適用
    public class PlayerPatch
    {
        // Prefixはメソッドの実行前に呼ばれます
        public static void Prefix(EntityPlayer __instance)
        {
            Debug.Log("Custom Player Model Loaded");

            // 既存のプレイヤーゲームオブジェクトにPlayerCharacterReplaceコンポーネントを追加
            GameObject playerObject = __instance.gameObject;
            var playerCharacterReplace = playerObject.AddComponent<PlayerCharacterReplace>();

            // プレハブの設定などを行う


            Debug.Log("Custom Player Model Initialized");
        }

    }
}
