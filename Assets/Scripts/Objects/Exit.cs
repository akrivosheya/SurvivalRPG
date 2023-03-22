using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class Exit : MonoBehaviour
{
    [SerializeField] private Transform NextExit;
    [SerializeField] private Vector3 Offset;
    private Collider2D[] _overlapedColliders = new Collider2D[1];
    private ObjectData _objectData;

    void Start()
    {
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Exit;
    }

    void Update()
    {
        var collider = GetComponent<BoxCollider2D>();
        var filter = new ContactFilter2D();
        if(collider.OverlapCollider(filter.NoFilter(), _overlapedColliders) > 0)
        {
            if(_overlapedColliders[0].gameObject.TryGetComponent<MovingPlayer>(out MovingPlayer moving) &&
            _overlapedColliders[0].gameObject.TryGetComponent<ObjectData>(out ObjectData objectData))
            {
                var nextPosition = NextExit.position + Offset;
                moving.SetTo(nextPosition);
            }
        }
    }
}
