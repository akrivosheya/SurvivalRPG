using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ObjectData))]

public class Interacting : MonoBehaviour
{
    [SerializeField] private float OverlapOffset = 0.1f;
    private BoxCollider2D _boxCollider;
    private ObjectData _objectData;
    private Vector2Int _direction;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _objectData = GetComponent<ObjectData>();
    }

    void Update()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["START_ENDING"] || Managers.Conditions["IS_PAUSE"])
        {
            return;
        }
        var axises = new Vector2Int();
        axises.x = (int)(Input.GetAxisRaw("Horizontal"));
        axises.y = (int)(Input.GetAxisRaw("Vertical"));
        if(!axises.Equals(Vector2Int.zero) && _direction != axises)
        {
            _direction = axises;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            var offset = new Vector3(_direction.x, _direction.y, 0);
            offset.x += Mathf.Sign(offset.x) * OverlapOffset;
            offset.y += Mathf.Sign(offset.y) * OverlapOffset;
            var maxBounds = _boxCollider.bounds.max;
            var minBounds = _boxCollider.bounds.min;
            var corner1 = new Vector2(maxBounds.x + offset.x, maxBounds.y + offset.y);
            var corner2 = new Vector2(minBounds.x + offset.x, minBounds.y + offset.y);
            Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
            if(hit != null)
            {
                if(hit.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    interactable.Interact((int)_objectData.Id);
                }
            }
        }
    }
}
