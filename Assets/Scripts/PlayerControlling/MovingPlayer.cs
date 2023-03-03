using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class MovingPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float SmoothingSpeed = 0.1f;
    [SerializeField] private float PositionRate = 0.5f;
    [SerializeField] private float ZYOffset = 0.2f;
    [SerializeField] private float AxisLimit = 0.5f;
    private Animator _animator;
    private ObjectData _objectData;
    private Vector3 _nextPosition;
    private Vector2Int _movement;
    private Vector3 _smoothingVelocity = Vector3.zero;
    private float _distanceToPosition;
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
        if(Managers.Dialogs.IsDialog || Managers.Conditions["START_ENDING"])
        {
            _animator.SetInteger("movement", (int)AnimatorParameters.Stop);
            return;
        }

        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");
        _movement.x = (int)((Mathf.Abs(xAxis) < AxisLimit) ? 0 : Mathf.Sign(xAxis));
        _movement.y = (int)((Mathf.Abs(yAxis) < AxisLimit) ? 0 : Mathf.Sign(yAxis));
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
        var newDistance = Vector2.Distance(transform.position, _nextPosition);
        if(newDistance < PositionRate)
        {
            if(newDistance > _distanceToPosition)
            {
                _isMoving = false;
            }
            else
            {
                _distanceToPosition = newDistance;
            }
        }
    }

    private void TryStartMoving()
    {
        if(_movement.Equals(Vector2Int.zero) || !Managers.Scene.TryMoveObject((int)_objectData.Id, _movement, out Vector2Int fixedMovement))
        {
            if(!transform.position.Equals(_nextPosition))
            {
                transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _smoothingVelocity, SmoothingSpeed);
            }
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
            if(_movement.Equals(Vector2Int.zero))
            {
                _animator.SetInteger("movement", (int)AnimatorParameters.Stop);
            }
            else if(_movement.x != 0)
            {    
                if(_movement.x > 0)
                {
                    _animator.SetInteger("movement", (int)AnimatorParameters.MovingRight);//плохо и дальше
                }
                else
                {
                    _animator.SetInteger("movement", (int)AnimatorParameters.MovingLeft);
                }
            }
            else if(_movement.y != 0)
            {
                if(_movement.y > 0)
                {
                    _animator.SetInteger("movement", (int)AnimatorParameters.MovingUp);
                }
                else
                {
                    _animator.SetInteger("movement", (int)AnimatorParameters.MovingDown);
                }
            }
        }
    }
}
