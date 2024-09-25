using UnityEngine;
using System;
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private float speed;

    void Start()
    {
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
    }

    void Update()
    {
        // キーボード入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D または ←/→キー
        float moveVertical = Input.GetAxis("Vertical");     // W/S または ↑/↓キー

        // 移動速度を計算
        speed = Mathf.Clamp01(Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));

        // Animatorのパラメータを更新
        animator.SetFloat("Speed", speed);
        animator.SetBool("isWalking", speed > 0.1f);
    }
}
