using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private float speed;

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator�R���|�[�l���g��������܂���I");
        }
    }

    void Update()
    {
        // �L�[�{�[�h���͂��擾
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D �܂��� ��/���L�[
        float moveVertical = Input.GetAxis("Vertical");     // W/S �܂��� ��/���L�[

        // �ړ����x���v�Z
        speed = Mathf.Clamp01(Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));

        // Animator�̃p�����[�^���X�V
        animator.SetFloat("Speed", speed);
        animator.SetBool("isWalking", speed > 0.1f);

        // �f�o�b�O�p���O
        Debug.Log($"moveHorizontal: {moveHorizontal}, moveVertical: {moveVertical}, speed: {speed}, isWalking: {speed > 0.1f}");
    }
}
