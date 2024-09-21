using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using UnityEngine.Windows;

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

[HarmonyPatch(typeof(GameManager))]
[HarmonyPatch("StartGame")]
public class GameManagerPatch
{
    public static void Postfix(GameManager __instance)
    {
        Debug.Log("World has been loaded.");

        // プレイヤーの初期化処理を開始
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
            var playerCharacterReplace = player.gameObject.GetComponent<PlayerCharacterReplace>();
            if (playerCharacterReplace == null)
            {
                playerCharacterReplace = player.gameObject.AddComponent<PlayerCharacterReplace>();
                Debug.Log("PlayerCharacterReplace component added to player.");
            }
        }
        else
        {
            Debug.LogError("Player not found.");
        }
    }
}

[HarmonyPatch(typeof(EntityPlayerLocal))]
[HarmonyPatch("Awake")]
public class PlayerPatch
{
    public static void Postfix(EntityPlayerLocal __instance)
    {
        Debug.Log("Player has spawned in world.");

        var playerCharacterReplace = __instance.gameObject.GetComponent<PlayerCharacterReplace>();
        if (playerCharacterReplace == null)
        {
            playerCharacterReplace = __instance.gameObject.AddComponent<PlayerCharacterReplace>();
            Debug.Log("PlayerCharacterReplace component added to player.");
        }
    }
}

public class PlayerCharacterReplace : MonoBehaviour
{
    public string UserProfilePath = "C:/Users/wakad/AppData/Roaming/7DaysToDie/Mods/AscensionProtocol";
    public string assetBundleName = "pink_twin.bundle"; // AssetBundleのパスを指定

    // VRoidモデルのPrefab（インポートしたキャラクター）
    public GameObject vroidCharacterPrefab;

    // プレイヤーの元のモデル（置き換え対象）
    private GameObject originalPlayerModel;

    // プレイヤーキャラクターの位置を保持するための参照
    private Transform playerTransform;

    // プレイヤーが初期化されたかどうかを確認するフラグ
    private bool isInitialized = false;

    [HarmonyPatch(typeof(EntityPlayerLocal))]  // EntityPlayerLocalクラスにパッチを適用（ローカルプレイヤー）
    [HarmonyPatch("Awake")]  // プレイヤーの開始処理にパッチを適用
    public class PlayerPatch
    {
        // Postfixはメソッドの実行後に呼ばれます
        public static void Postfix(EntityPlayerLocal __instance)
        {
            Debug.Log("Custom Player Model Loaded");

            // 既存のプレイヤーゲームオブジェクトにPlayerCharacterReplaceコンポーネントを追加
            GameObject playerObject = __instance.gameObject;
            var playerCharacterReplace = playerObject.GetComponent<PlayerCharacterReplace>();
            if (playerCharacterReplace == null)
            {
                playerCharacterReplace = playerObject.AddComponent<PlayerCharacterReplace>();
            }

            // プレハブの設定などを行う（必要に応じて）

            Debug.Log("Custom Player Model Initialized");
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // プレイヤーモデルがシーンにロードされるのを待つ
        yield return new WaitForSeconds(1.0f); // 0.5秒待機

        // プレイヤーの元のモデルを取得
        ReplacePlayerCharacter(GameManager.Instance.World.GetPrimaryPlayer() as EntityPlayerLocal);
    }

    // プレイヤーがロードされるまで待機
    void Update()
    {
        if (!isInitialized)
        {
            EntityPlayerLocal player = GameManager.Instance.World?.GetPrimaryPlayer();
            if (player != null && player.IsAlive())
            {
                isInitialized = true;
                StartCoroutine(ReplacePlayerCharacter(player));
            }
        }
    }

    private IEnumerator ReplacePlayerCharacter(EntityPlayerLocal player)
    {
        Debug.Log("Initializing PlayerCharacterReplace");

        if (player == null)
        {
            Debug.LogError("Player is null.");
            yield break;
        }
        else
        {
            Debug.Log("Player is not null.");
        }

        // フィールドに値を割り当てる
        originalPlayerModel = player.gameObject;
        if (originalPlayerModel == null)
        {
            Debug.LogError("Original player model is null.");
            yield break;
        }
        else
        {
            Debug.Log("Original player model is not null.");
        }

        // プレハブを読み込む
        yield return StartCoroutine(LoadCharacterFromAssetBundleAsync());

        if (vroidCharacterPrefab == null)
        {
            Debug.LogError("VRoidキャラクタープレハブの読み込みに失敗しました。");
            yield break;
        }
        else
        {
            Debug.Log("VRoidキャラクタープレハブの読み込みに成功しました。");
        }

        // プレイヤーのTransformを取得
        Transform playerTransform = originalPlayerModel.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is null.");
            yield break;
        }
        else
        {
            Debug.Log("Player transform is not null.");
        }

        // 元のプレイヤーモデルの見た目部分を非表示にする
        HideOriginalPlayerModel(player);

        // カスタムモデルをインスタンス化
        Debug.Log("Instantiate VRoid model");
        GameObject newPlayerModel = Instantiate(vroidCharacterPrefab, playerTransform.position, playerTransform.rotation);
        if (newPlayerModel == null)
        {
            Debug.LogError("Failed to instantiate VRoid model.");
            yield break;
        }
        else
        {
            Debug.Log("VRoid model instantiated successfully");
        }

        // モデルの設定
        // モデルをプレイヤーの子オブジェクトに設定
        newPlayerModel.transform.SetParent(player.transform, false);

        newPlayerModel.transform.localPosition = Vector3.zero;
        newPlayerModel.transform.localRotation = Quaternion.identity;
        newPlayerModel.transform.localScale = Vector3.one;

        // モデルのローカル位置を調整
        newPlayerModel.transform.localPosition = new Vector3(0, 0, 0.5f); // Z軸方向に0.5f前方に移動

        Debug.Log("Model position, rotation, scale set.");

        // レイヤー設定
        SetLayerRecursively(newPlayerModel, playerTransform.gameObject.layer);

        // アニメーションの設定
        SetupPlayerAnimations(newPlayerModel, player);
    }

    void HideOriginalPlayerModel(EntityPlayerLocal player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null in HideOriginalPlayerModel.");
            return;
        }

        // プレイヤーの子オブジェクト名を出力
        Debug.Log("Player's child objects:");
        PrintChildTransforms(player.transform);

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

    void PrintChildTransforms(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("Child: " + child.name);
        }
    }


    // プレイヤーにアニメーションを設定する関数
    void SetupPlayerAnimations(GameObject newPlayerModel, EntityPlayerLocal player)
    {
        if (newPlayerModel == null)
        {
            Debug.LogError("New player model is null in SetupPlayerAnimations.");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player is null in SetupPlayerAnimations.");
            return;
        }

        Animator playerAnimator = newPlayerModel.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            playerAnimator = newPlayerModel.AddComponent<Animator>();
            Debug.Log("Animatorコンポーネントを追加しました。");
        }

        // VRoidモデルのアバターを使用
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

        // オリジナルのAnimator Controllerを設定
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

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
            return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
                continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public IEnumerator LoadCharacterFromAssetBundleAsync()
    {
        string assetBundlePath = Path.Combine(UserProfilePath, assetBundleName);
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
        yield return request;

        AssetBundle bundle = request.assetBundle;
        if (bundle != null)
        {
            string prefabName = "pink_twin";
            AssetBundleRequest prefabRequest = bundle.LoadAssetAsync<GameObject>(prefabName);
            yield return prefabRequest;

            GameObject prefab = prefabRequest.asset as GameObject;
            if (prefab != null)
            {
                vroidCharacterPrefab = prefab;
                Debug.Log("VRoidキャラクタープレハブの読み込みに成功しました。");
            }
            else
            {
                Debug.LogError("プレハブがバンドルに存在しません。");
            }
        }
        else
        {
            Debug.LogError("アセットバンドルのロードに失敗しました。パスを確認してください。");
        }
    }
}
