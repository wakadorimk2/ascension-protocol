using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class MyModAPI : IModApi
{
    public void InitMod(Mod _modInstance)
    {
        Debug.Log("Initializing Mod API");

        // モデルを置き換える
        var harmony = new Harmony("com.ascension-protocol.mod");
        harmony.PatchAll();
    }
}

public class PlayerCharacterReplace : MonoBehaviour
{
    // VRoidモデルのPrefab（インポートしたキャラクター）
    public GameObject vroidCharacterPrefab;

    // プレイヤーの元のモデル（置き換え対象）
    private GameObject originalPlayerModel;

    // プレイヤーキャラクターの位置を保持するための参照
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの元のモデルを取得
        originalPlayerModel = GameObject.FindWithTag("PlayerModel");

        if (originalPlayerModel != null)
        {
            // プレイヤーのTransformを取得（位置・回転・スケールを保持する）
            playerTransform = originalPlayerModel.transform;

            // 元のプレイヤーモデルを非表示または削除
            originalPlayerModel.SetActive(false);

            // VRoidキャラクターをプレイヤーの位置に生成
            GameObject newPlayerModel = Instantiate(vroidCharacterPrefab, playerTransform.position, playerTransform.rotation);

            // 生成されたVRoidキャラクターをプレイヤーに追従させる
            newPlayerModel.transform.SetParent(playerTransform);

            // アニメーションやリグの設定をプレイヤーに対応させる
            SetupPlayerAnimations(newPlayerModel);
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
        playerCharacterReplace.vroidCharacterPrefab = Resources.Load<GameObject>("pink_twin");
        if (playerCharacterReplace.vroidCharacterPrefab == null)
        {
            Debug.LogError("VRoidキャラクタープレハブが読み込まれませんでした。パスを確認してください。");
        }

        Debug.Log("Custom Player Model Initialized");
    }
}
