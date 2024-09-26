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

        lastPosition = transform.position;
        Debug.Log("PlayerAnimationController初期化完了");
    }

    void FixedUpdate()
    {
        if (playerEntity == null || animator == null)
        {
            return;
        }

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

public class EntityPlayerLocalWrapper : IPlayerEntity
{
    private EntityPlayerLocal player;

    public EntityPlayerLocalWrapper(EntityPlayerLocal player)
    {
        this.player = player;
    }

    public Vector3 GetPosition() => player.position;
    public Vector3 GetVelocity() => player.motion;
    // 他の必要なメソッドの実装
}

public interface IPlayerEntity
{
    Vector3 GetPosition();
    Vector3 GetVelocity();
    // 他の必要なメソッドやプロパティ
}