using UnityEngine;
using System;

public class PlayerAnimationControllerBehaviour : MonoBehaviour
{
    private Animator animator;
    private Animator vroidAnimator;
    private EntityPlayerLocal playerEntity;
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

        Debug.Log("PlayerAnimationControllerが初期化されました。");
    }

    void Update()
    {
        if (playerEntity == null || animator == null) return;

        // 5秒ごとにログを出力
        if (Time.time - lastLogTime >= 5.0f)
        {
            // Forwardパラメータの値をログ出力
            Debug.Log($"Forward: {animator.GetFloat("Forward")}");

            lastLogTime = Time.time;
        }
    }
}

// AnimatorControllerParameterの一覧
// Animator parameter: Forward type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: Strafe type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: JumpTrigger type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: LookAngle type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WeaponFire type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WeaponPreFire type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WeaponPreFireCancel type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WeaponHoldType type: Int defaultFloat: 0 defaultInt: 45 defaultBool: False
// Animator parameter: IsAlive type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: ItemUse type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsFPV type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: Reload type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsCrouching type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsAiming type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsMale type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IdleTime type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsMoving type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: HitBodyPart type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: HitDirection type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: RotationPitch type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: HitDamage type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WalkType type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: VerticalSpeed type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: IsClimbing type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: MovementState type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: drunk type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: ItemHasChangedTrigger type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: Harvesting type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: Revive type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: ReloadSpeed type: Float defaultFloat: 2 defaultInt: 0 defaultBool: False
// Animator parameter: PowerAttack type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: CancelAttack type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: MeleeAttackSpeed type: Float defaultFloat: 1 defaultInt: 0 defaultBool: False
// Animator parameter: RayHit type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: ItemActionIndex type: Int defaultFloat: 0 defaultInt: -1 defaultBool: False
// Animator parameter: UseItem type: Trigger defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: YLook type: Float defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: WeaponAmmoRemaining type: Int defaultFloat: 0 defaultInt: 0 defaultBool: False
// Animator parameter: Holstered type: Bool defaultFloat: 0 defaultInt: 0 defaultBool: False
