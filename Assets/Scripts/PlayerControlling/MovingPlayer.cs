using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
    public int Id { get; private set; } = 2;//лучше где-то взять
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float SmoothingSpeed = 0.1f;
    [SerializeField] private float PositionRate = 0.5f;
    private Animator _animator;
    private Vector3 _nextPosition;
    public Vector3Int _movement;//должен быть private
    public Vector2Int directionA;
    private Vector3 _smoothingVelocity = Vector3.zero;
    private float _distanceToPosition;
    private bool _isMoving = false;
    private bool _noDirection = false;

    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
        _nextPosition = transform.position;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["START_ENDING"])
        {
            _animator.SetInteger("direction", 0);
            return;
        }
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");
            var direction = new Vector2Int();
            direction.x = (int)((Mathf.Approximately(xAxis, 0)) ? 0 : Mathf.Sign(xAxis));
            direction.y = (int)((Mathf.Approximately(yAxis, 0)) ? 0 : Mathf.Sign(yAxis));
            directionA = direction;
            if(_movement.x != direction.x || _movement.y != direction.y)
            {
                if(!direction.Equals(Vector2Int.zero))
                {
                    _movement = new Vector3Int(direction.x, direction.y, 0);
                }
                else
                {
                    _noDirection = true;
                    _animator.SetInteger("direction", 0);
                }
            }
        if(_isMoving)
        {
            var oldPosition = transform.position;
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
                    //_animator.SetInteger("direction", 0);//плохо
                    /*transform.position = _nextPosition;
                }
                else
                {
                    _distanceToPosition = newDistance;*/
                }
            }
        }
        else
        {
            if(Mathf.Approximately(xAxis, 0) && Mathf.Approximately(yAxis, 0))
            {
                //_animator.SetInteger("direction", 0);//плохо
                if(!transform.position.Equals(_nextPosition))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _smoothingVelocity, SmoothingSpeed);
                }
                return;
            }
            if(!Managers.Scene.TryMoveObject(Id, direction, out Vector2Int fixedDirection))
            {
                //_animator.SetInteger("direction", 0);//плохо
                if(!transform.position.Equals(_nextPosition))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _smoothingVelocity, SmoothingSpeed);
                }
                return;
            }
            _isMoving = true;
            _nextPosition = Managers.Scene.GetObjectScenePosition(Id);
            _nextPosition.z = _nextPosition.y;
        }
    }

    void LateUpdate()
    {
        
            if(!_noDirection && !Managers.Dialogs.IsDialog && !Managers.Conditions["START_ENDING"])//очень плохо
            {
                _noDirection = false;
                if(_movement.x != 0)
                {    
                    if(_movement.x > 0)
                    {
                        _animator.SetInteger("direction", 3);//плохо и дальше
                    }
                    else
                    {
                        _animator.SetInteger("direction", 4);
                    }
                }
                else if(_movement.y != 0)
                {
                    if(_movement.y > 0)
                    {
                        _animator.SetInteger("direction", 2);
                    }
                    else
                    {
                        _animator.SetInteger("direction", 1);
                    }
                }
            }
            _noDirection = false;
    }

    public void MoveTo(Vector3 transformPosition)
    {
        _isMoving = true;
        _nextPosition = Managers.Scene.GetObjectScenePosition(Id);
    }

    public void SetTo(Vector3 transformPosition)
    {
        _isMoving = false;
        transform.position = transformPosition;
        _nextPosition = transformPosition;
    }
}
