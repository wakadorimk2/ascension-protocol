// Utilities.cs
using UnityEngine;

namespace Common
{
    public static class Utilities
    {
        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (obj == null)
                return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (child == null)
                    continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        public static void LogAnimatorParameters(Animator animator)
        {
            if (animator == null)
            {
                Debug.LogError("Animator is null.");
                return;
            }

            AnimatorControllerParameter[] parameters = animator.parameters;

            Debug.Log($"Animator has {parameters.Length} parameters:");
            foreach (var param in parameters)
            {
                Debug.Log($"- {param.type}: {param.name}");
            }
        }

        public static void LogAnimatorClips(Animator animator)
        {
            if (animator == null)
            {
                Debug.LogError("Animator is null.");
                return;
            }

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            if (controller == null)
            {
                Debug.LogError("RuntimeAnimatorController is null.");
                return;
            }

            AnimationClip[] clips = controller.animationClips;
            Debug.Log($"Animator has {clips.Length} animation clips:");
            foreach (var clip in clips)
            {
                Debug.Log($"- {clip.name}");
            }
        }
    }
}
