using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// Smoothly follows a target in the XY plane. The camera keeps its own z.
    /// </summary>
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        [Tooltip("How fast the camera closes the gap to the target, per second. Higher follows more tightly.")]
        private float _smoothing = 2f;

        [SerializeField]
        [Tooltip("The transform the camera follows.")]
        private Transform _target;

        private void Awake()
        {
            if (_target == null)
            {
                Debug.LogError($"{nameof(CameraFollow)} on '{name}' has no target.", this);
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = _target.position;
            Vector3 goal = new Vector3(targetPosition.x, targetPosition.y, position.z);

            // Exponential smoothing: the share of the gap closed this frame does not depend on the frame rate.
            float t = 1f - Mathf.Exp(-_smoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(position, goal, t);
        }
    }
}
