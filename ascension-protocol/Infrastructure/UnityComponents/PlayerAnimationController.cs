using UnityEngine;
using System;
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private EntityPlayerLocal playerEntity;
    private Vector3 lastPosition;
    private float speed;
    private bool isWalking;
    private float lastLogTime;

    void Start()
    {
        // EntityPlayerLocalコンポーネントを取得
        playerEntity = GetComponent<EntityPlayerLocal>();
        if (playerEntity == null)
        {
            Debug.LogError("EntityPlayerLocalコンポーネントが見つかりません！");
            throw new Exception("EntityPlayerLocalコンポーネントが見つかりません！");
        }
        else
        {
            Debug.Log("EntityPlayerLocalコンポーネントを取得しました。");
        }

        // Animatorコンポーネントを取得
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません！");
            throw new Exception("Animatorコンポーネントが見つかりません！");
        }
        else
        {
            Debug.Log("Animatorコンポーネントを取得しました。");
        }

        // Animator Controllerの確認
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Animator Controllerが設定されていません！");
            throw new Exception("Animator Controllerが設定されていません！");
        }
        else
        {
            Debug.Log("Animator Controllerが設定されています。");
        }

        lastPosition = transform.position;
        Debug.Log("PlayerAnimationController初期化完了");
    }

    void FixedUpdate()
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