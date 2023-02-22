using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Interacting : MonoBehaviour
{
    [SerializeField] private InteractigBoxSize;
    private BoxCollider2D _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            var maxBounds = _boxCollider.bounds.max;
            var minBounds = _boxCollider.bounds.min;
            var corenr1 = new Vector2(maxBounds.x + InteractigBoxSize.x, maxBounds.y);
            var corenr2 = new Vector2(minBounds.x, minBounds.y);
            Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        }
    }
}
