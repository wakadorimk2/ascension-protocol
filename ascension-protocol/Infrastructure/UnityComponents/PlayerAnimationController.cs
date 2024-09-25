using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private float speed;

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません！");
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

        // デバッグ用ログ
        Debug.Log($"moveHorizontal: {moveHorizontal}, moveVertical: {moveVertical}, speed: {speed}, isWalking: {speed > 0.1f}");
    }
}
