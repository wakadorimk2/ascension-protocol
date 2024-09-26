using UnityEngine;
using System;
public class PlayerAnimationControllerBehaviour : MonoBehaviour
{
    private Animator animator;
    private EntityPlayerLocal playerEntity;
    private Vector3 lastPosition;
    private float speed;
    private bool isWalking;
    private float lastLogTime;

    void Awake()
    {
        Debug.Log("PlayerAnimationControllerBehaviour Awake called.");

        // EntityPlayerLocalコンポーネントを取得
        playerEntity = GetComponent<EntityPlayerLocal>();
        if (playerEntity == null)
        {
            throw new Exception("EntityPlayerLocalコンポーネントが見つかりません！");
        }

        // EntityPlayerLocalコンポーネントの情報を表示
        Debug.Log("EntityPlayerLocal: " + playerEntity.name);

        // Animatorコンポーネントを取得
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            throw new Exception("Animatorコンポーネントが見つかりません！");
        }

        // Animatorコンポーネントの情報を表示
        Debug.Log("Animator: " + animator.name);

        // Animator Controllerの確認
        if (animator.runtimeAnimatorController == null)
        {
            throw new Exception("Animator Controllerが設定されていません！");
        }

        // Animator Controllerの情報を表示
        Debug.Log("Animator Controller: " + animator.runtimeAnimatorController.name);

        lastPosition = transform.position;
        Debug.Log("PlayerAnimationControllerが初期化されました。");
    }

    void Update()
    {
        if (playerEntity == null || animator == null) return;

        // 現在位置と前回位置の差分から速度を計算
        Vector3 displacement = transform.position - lastPosition;
        speed = displacement.magnitude / Time.fixedDeltaTime;

        // 歩行状態を更新
        isWalking = speed > 0.1f;

        // Animatorのパラメータを更新
        animator.SetFloat("Speed", speed);
        animator.SetBool("isWalking", isWalking);

        // 位置を更新
        lastPosition = transform.position;

        // 1秒ごとにログを出力
        if (Time.time - lastLogTime >= 1.0f)
        {
            Debug.Log($"Speed: {speed:F2} isWalking: {isWalking} position: {transform.position}");
            lastLogTime = Time.time;
        }
    }
}