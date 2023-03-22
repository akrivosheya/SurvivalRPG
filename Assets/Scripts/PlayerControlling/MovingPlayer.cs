using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class MovingPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 StartPosition;
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float ZYOffset = 0.2f;
    private Animator _animator;
    private Collider2D[] _overlapedColliders = new Collider2D[1];
    private BoxCollider2D _collider;
    private ContactFilter2D _filter = new ContactFilter2D();
    [SerializeField] private List<MovingNPC> _npcs = new List<MovingNPC>();
    private ObjectData _objectData;
    private Vector3 _nextPosition;
    private Vector2Int _movement;
    private Vector2Int _fixedMovement;
    private bool _isMoving = false;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Player;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, StartPosition);
        _nextPosition = StartPosition;
        _isMoving = true;
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
        foreach(var npc in _npcs)
        {
            npc.SetTo(transformPosition);
        }
    }

    public void AddNpc(GameObject npcPrefab)
    {
        var newNpc = Instantiate(npcPrefab);
        newNpc.transform.position = transform.position;
        _npcs.Add(newNpc.GetComponent<MovingNPC>());//проверки
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
