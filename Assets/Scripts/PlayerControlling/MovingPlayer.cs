using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class MovingPlayer : MonoBehaviour
{
    [SerializeField] private GameObject NPCPrefab;
    [SerializeField] private Camera Camera;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 StartPosition;
    [SerializeField] private float CameraZPosition = -10;
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float ZYOffset = 0.2f;
    private Collider2D[] _overlapedColliders = new Collider2D[1];
    private BoxCollider2D _collider;
    private ContactFilter2D _filter = new ContactFilter2D();
    private List<MovingNPC> _npcs = new List<MovingNPC>();
    private ObjectData _objectData;
    private Vector3 _nextPosition;
    private Vector2Int _movement;
    private Vector2Int _fixedMovement;
    private Vector2Int _lastNotZeroMovement;
    private bool _isMoving = false;
    private bool _prevIsMoving = false;

    void Awake()
    {
        Messenger.AddListener(GameEvent.JOIN_TO_GROUP, OnJoinToGroup);
        Messenger.AddListener(GameEvent.EXIT_FROM_GROUP, OnExitFromGroup);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.JOIN_TO_GROUP, OnJoinToGroup);
        Messenger.RemoveListener(GameEvent.EXIT_FROM_GROUP, OnExitFromGroup);
    }

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Player;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, StartPosition);
        _nextPosition = StartPosition;
        _isMoving = true;
    }

    void Update()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["SCENE_IS_CHANGING"] || Managers.Conditions["IS_PAUSE"])
        {
            return;
        }
        _movement.x = (int)(Input.GetAxisRaw("Horizontal"));
        _movement.y = (int)(Input.GetAxisRaw("Vertical"));
        if(!_movement.Equals(Vector2Int.zero))
        {
            _lastNotZeroMovement = _movement;
        }
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
        Managers.Scene.DeleteObject((int)_objectData.Id);
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transformPosition);
        _isMoving = false;
        transform.position = transformPosition;
        _nextPosition = transformPosition;
        foreach(var npc in _npcs)
        {
            npc.SetTo(transformPosition);
        }
        transformPosition.z = CameraZPosition;
        Camera.transform.position = transformPosition;
    }

    public void OnJoinToGroup()
    {
        var newNpc = Instantiate(NPCPrefab);
        newNpc.transform.position = transform.position;
        _npcs.Add(newNpc.GetComponent<MovingNPC>());//проверки
    }

    public void OnExitFromGroup()
    {
        foreach(var npc in _npcs)
        {
            Destroy(npc.gameObject);
        }
        _npcs.Clear();
    }

    private void Move()//м/б отдельным компонентом
    {
        var movement = _nextPosition - transform.position;
        movement.Normalize();
        var currentSpeed = GetCurrentSpeed();
        movement = movement * currentSpeed * Time.deltaTime;
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
        if(!Managers.Dialogs.IsDialog && !Managers.Conditions["SCENE_IS_CHANGING"])//очень плохо
        {
            var movement = new Vector2Int();
            if(_isMoving)
            {
                movement = _movement;
            }
            else
            {
                movement = _lastNotZeroMovement;
            }
            animator.SetInteger("directionX", movement.x);
            if(_movement.x == 0)
            {
                animator.SetInteger("directionY", movement.y);
            }
            else
            {
                animator.SetInteger("directionY", 0);
            }
            //animator.Update(Time.deltaTime);
            animator.SetBool("isMoving", _isMoving);
            if(_prevIsMoving != _isMoving)
            {
                animator.Update(Time.deltaTime);
            }
            _prevIsMoving = _isMoving;
        }
    }
}
