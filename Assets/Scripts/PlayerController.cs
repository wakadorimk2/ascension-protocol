using UnityEngine;

namespace AscensionProtocol.Infrastructure.UnityComponents
{
    public class PlayerController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("W key was pressed");
            }
        }
    }
}
