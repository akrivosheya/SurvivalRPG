using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class Wall : MonoBehaviour
{
    private ObjectData _objectData;

    void Start()
    {
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Wall;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
    }
}
