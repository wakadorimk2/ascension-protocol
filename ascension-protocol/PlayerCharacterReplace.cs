﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

            // さらに、アニメーションやリグの設定をプレイヤーに対応させる処理を追加（以下、アニメーション設定）
            SetupPlayerAnimations(newPlayerModel);
        }
        else
        {
            Debug.LogError("元のプレイヤーモデルが見つかりません。タグを確認してください。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーのアニメーションを制御するための関数
    }
    // プレイヤーにアニメーションを設定する関数
    void SetupPlayerAnimations(GameObject newPlayerModel)
    {
        Animator playerAnimator = newPlayerModel.GetComponent<Animator>();

        if (playerAnimator != null)
        {
            // 必要に応じてアニメーションコントローラーを設定
            // ここに既存のプレイヤーのアニメーションを適用する処理を記述
            playerAnimator.runtimeAnimatorController = Resources.Load("PlayerAnimatorController") as RuntimeAnimatorController;
        }
        else
        {
            Debug.LogError("VRoidモデルにAnimatorがありません。リグ設定を確認してください。");
        }
    }
}
