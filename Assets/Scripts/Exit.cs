using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public int Id { get; private set; } = 4;
    [SerializeField] private Transform NextExit;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private List<string> ConditionsToExit;
    private Collider2D[] _overlapedColliders = new Collider2D[1];

    void Start()
    {
        //Managers.Scene.SetObjectPosition(Id, transform.position);
    }

    void Update()
    {
        var collider = GetComponent<BoxCollider2D>();
        var filter = new ContactFilter2D();
        if(collider.OverlapCollider(filter.NoFilter(), _overlapedColliders) > 0)
        {
            if(_overlapedColliders[0].gameObject.TryGetComponent<MovingPlayer>(out MovingPlayer moving))
            {
                var correctConditions = true;
                foreach(var condition in ConditionsToExit)
                {
                    if(!Managers.Conditions[condition])
                    {
                        correctConditions = false;
                        break;
                    }
                }
                if(correctConditions)
                {
                    Managers.Levels.LoadScene(-1);//что-то
                }
                //moving.transform.position = NextExit.position;
                moving.SetTo(NextExit.position + Offset);
                Managers.Scene.DeleteObject(moving.Id);
                Managers.Scene.SetObjectPosition(moving.Id, NextExit.position + Offset);
            }
        }
    }
}
