using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class MovingPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float ZYOffset = 0.2f;
    private Animator _animator;
    private ObjectData _objectData;
    private Vector3 _nextPosition;
    private Vector2Int _movement;
    private Vector2Int _fixedMovement;
    private bool _isMoving = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Player;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
        _nextPosition = transform.position;
    }

    void Update()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["START_ENDING"] || Managers.Conditions["IS_PAUSE"])
        {
            return;
        }
        _movement.x = (int)(Input.GetAxisRaw("Horizontal"));
        _movement.y = (int)(Input.GetAxisRaw("Vertical"));
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

    public void MoveTo(Vector3 transformPosition)
    {
        _isMoving = true;
        _nextPosition = Managers.Scene.GetObjectScenePosition((int)_objectData.Id);
    }

    public void SetTo(Vector3 transformPosition)
    {
        _isMoving = false;
        transform.position = transformPosition;
        _nextPosition = transformPosition;
    }

    private void Move()
    {
        var movement = _nextPosition - transform.position;
        movement.Normalize();
        movement = movement * Speed * Time.deltaTime;
        transform.Translate(movement);
        if(Vector3.Dot(movement, _nextPosition - transform.position) <= 0)
        {
            if(_movement.Equals(Vector2Int.zero))
            {
                transform.position = _nextPosition;
                _isMoving = false;
            }
            else
            {
                TryStartMoving();
            }
        }
    }

    private void TryStartMoving()
    {
        if(_movement.Equals(Vector2Int.zero) || !Managers.Scene.TryMoveObject((int)_objectData.Id, _movement, out _fixedMovement))
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
            _nextPosition = Managers.Scene.GetObjectScenePosition((int)_objectData.Id);
            _nextPosition.z = _nextPosition.y + ZYOffset;
        }
    }

    private void SetAnimation()
    {
        if(!Managers.Dialogs.IsDialog && !Managers.Conditions["START_ENDING"])//очень плохо
        {
            _animator.SetInteger("directionX", _movement.x);
            if(_movement.x == 0)
            {
                _animator.SetInteger("directionY", _movement.y);
            }
            else
            {
                _animator.SetInteger("directionY", 0);
            }
            _animator.SetBool("isMoving", _isMoving);
        }
    }
}
