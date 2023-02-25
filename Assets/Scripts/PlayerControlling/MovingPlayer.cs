using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
    public int Id { get; private set; } = 2;//лучше где-то взять
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float SmoothingSpeed = 0.1f;
    [SerializeField] private float PositionRate = 0.5f;
    private Vector3 _nextPosition;
    public Vector3 _movement;//должен быть private
    private Vector3 _smoothingVelocity = Vector3.zero;
    private float _distanceToPosition;
    private bool _isMoving = false;

    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
        _nextPosition = transform.position;
    }

    void Update()
    {
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
            if(Managers.Dialogs.IsDialog)
            {
                return;
            }
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");
            if(Mathf.Approximately(xAxis, 0) && Mathf.Approximately(yAxis, 0))
            {
                if(!transform.position.Equals(_nextPosition))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _smoothingVelocity, SmoothingSpeed);
                }
                return;
            }
            var direction = new Vector2Int();
            direction.x = (int)((Mathf.Approximately(xAxis, 0)) ? 0 : Mathf.Sign(xAxis));
            direction.y = (int)((Mathf.Approximately(yAxis, 0)) ? 0 : Mathf.Sign(yAxis));
            if(!Managers.Scene.TryMoveObject(Id, direction, out Vector2Int fixedDirection))
            {
                if(!transform.position.Equals(_nextPosition))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _smoothingVelocity, SmoothingSpeed);
                }
                return;
            }
            _movement = new Vector3(fixedDirection.x, fixedDirection.y, 0);
            _isMoving = true;
            _nextPosition = Managers.Scene.GetObjectScenePosition(Id);
        }
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
