using UnityEngine;
using System;
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private IPlayerEntity playerEntity;
    private float lastLogTime;

    void Start()
    {
        // EntityPlayerLocalコンポーネントを取得
        playerEntity = GetComponent<IPlayerEntity>();
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

        // check if animator is null
        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません！");
            throw new Exception("Animatorコンポーネントが見つかりません！");
        }
        else
        {
            Debug.Log("Animatorコンポーネントを取得しました。");
        }

        Debug.Log("PlayerAnimationController初期化完了");
    }

    void FixedUpdate()
    {
        if (playerEntity == null || animator == null)
        {
            return;
        }

        Vector3 velocity = playerEntity.GetVelocity();
        float speed = velocity.magnitude;

        // 歩行状態を更新
        bool isWalking = speed > 0.1f;

        // Animatorのパラメータを更新
        animator.SetFloat("Speed", speed);
        animator.SetBool("isWalking", isWalking);

        // 1秒ごとにログを出力
        if (Time.time - lastLogTime >= 1.0f)
        {
            Debug.Log($"Speed: {speed:F2} isWalking: {isWalking} position: {transform.position}");
            lastLogTime = Time.time;
        }
    }
}

public interface IPlayerEntity
{
    Vector3 GetPosition();
    Vector3 GetVelocity();
    // 他の必要なメソッドやプロパティ
}