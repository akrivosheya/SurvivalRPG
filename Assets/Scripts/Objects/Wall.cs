using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class Wall : MonoBehaviour
{
    [SerializeField] private float Offset;
    private ObjectData _objectData;

    void Start()
    {
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Wall;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + Offset);
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
    }
}
