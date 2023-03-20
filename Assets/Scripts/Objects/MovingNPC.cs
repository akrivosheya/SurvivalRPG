using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class MovingNPC : MonoBehaviour
{
    [SerializeField] private Vector2 AxisLimit = new Vector2(0.01f, 0.01f);
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float ZYOffset = 0.2f;
    [SerializeField] private int TargetId;
    private Animator _animator;
    private BoxCollider2D _collider;
    private ContactFilter2D _filter = new ContactFilter2D();
    private Collider2D[] _overlapedColliders = new Collider2D[1];
    private ObjectData _objectData;
    private Vector3 _oldTargetPosition;
    private Vector3 _nextPosition;
    private Vector3 _movement = Vector3.zero;
    private bool _isMoving = false;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.NPC;
        Managers.Scene.RawSetObjectPosition((int)_objectData.Id, transform.position);
        _nextPosition = transform.position;
        _oldTargetPosition = transform.position;
    }

    void Update()
    {
        if(_isMoving)
        {
            Move();
        }
        else
        {
            TryStartMoving();
        }
        SetAnimation();
    }

    private void Move()
    {
        _movement = _nextPosition - transform.position;
        _movement.Normalize();
        var currentSpeed = GetCurrentSpeed();
        _movement = _movement * currentSpeed * Time.deltaTime;
        transform.Translate(_movement);
        if(Vector3.Dot(_movement, _nextPosition - transform.position) <= 0)
        {
            TryStartMoving();
        }
    }

    public void SetTo(Vector3 transformPosition)
    {
        _isMoving = false;
        transform.position = transformPosition;
        _nextPosition = transformPosition;
        _oldTargetPosition = transform.position;
    }

    private void TryStartMoving()
    {
        var currentTargetPosition = Managers.Scene.GetObjectScenePosition(TargetId);
        if(currentTargetPosition.Equals(_oldTargetPosition))
        {
            _isMoving = false;
            _movement = Vector3.zero;
            transform.position = _nextPosition;
        }
        else
        {
            _isMoving = true;
            _nextPosition = _oldTargetPosition;
            _oldTargetPosition = currentTargetPosition;
            Managers.Scene.RawSetObjectPosition((int)_objectData.Id, currentTargetPosition);
            _nextPosition.z = _nextPosition.y + ZYOffset;
        }
    }

    private float GetCurrentSpeed()
    {
        if(_collider.OverlapCollider(_filter.NoFilter(), _overlapedColliders) > 0)
        {
            if(_overlapedColliders[0].gameObject.TryGetComponent<SpeedSetter>(out SpeedSetter speedSetter))
            {
                return speedSetter.Speed;
            }
        }
        return Speed;
    }

    private void SetAnimation()
    {
        if(!Managers.Dialogs.IsDialog && !Managers.Conditions["START_ENDING"])//очень плохо
        {
            var _integerMovement = new Vector2Int();
            _integerMovement.x = (_movement.x > AxisLimit.x) ? 1 : ((_movement.x < -AxisLimit.x) ? -1 : 0);
            _integerMovement.y = (_movement.y > AxisLimit.y) ? 1 : ((_movement.y < -AxisLimit.y) ? -1 : 0);
            _animator.SetInteger("directionX", _integerMovement.x);
            if(_integerMovement.x == 0)
            {
                _animator.SetInteger("directionY", _integerMovement.y);
            }
            else
            {
                _animator.SetInteger("directionY", 0);
            }
            _animator.SetBool("isMoving", _isMoving);
        }
    }
}
