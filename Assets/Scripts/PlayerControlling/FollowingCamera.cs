using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float SmoothTime = 0.2f;

    private Vector3 _velocity = Vector3.zero;

    void LateUpdate()
    {
        var targetPosition = Target.position;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime);
    }
}
