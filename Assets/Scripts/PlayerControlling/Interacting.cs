using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Interacting : MonoBehaviour
{
    private BoxCollider2D _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["START_ENDING"])
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            var movementInt = GetComponent<MovingPlayer>()._movement;//надо заменить
            var movement = new Vector3(movementInt.x, movementInt.y, 0);
            movement.x += Mathf.Sign(movement.x) * 0.1f;
            movement.y += Mathf.Sign(movement.y) * 0.1f;
            var maxBounds = _boxCollider.bounds.max;
            var minBounds = _boxCollider.bounds.min;
            var corner1 = new Vector2(maxBounds.x + movement.x, maxBounds.y + movement.y);
            var corner2 = new Vector2(minBounds.x + movement.x, minBounds.y + movement.y);
            Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
            if(hit != null)
            {
                if(hit.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    interactable.Interact(GetComponent<MovingPlayer>().Id);//заменить
                }
            }
        }
    }
}
