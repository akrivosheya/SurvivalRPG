using UnityEngine;

public class FollowingDarkness : MonoBehaviour
{
    [SerializeField] private Transform Target;

    void LateUpdate()
    {
        transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
    }
}
