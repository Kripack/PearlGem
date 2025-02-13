using UnityEngine;

namespace PearlGem.Utils
{
    public class RotateComponent : MonoBehaviour
    {
        [SerializeField] bool rotate = true;
        [SerializeField] float rotationSpeed = 45f;
        [SerializeField] Vector3 rotationAxis = Vector3.up;

        void Update()
        {
            if(rotate)
            {
                float angle = rotationSpeed * Time.unscaledDeltaTime;
                transform.Rotate(rotationAxis, angle, Space.World);
            }
        }
    }
}