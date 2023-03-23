using UnityEngine;

public class ZOffsetSetter : MonoBehaviour
{[SerializeField] private float Offset = 1f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + Offset);
    }
}
