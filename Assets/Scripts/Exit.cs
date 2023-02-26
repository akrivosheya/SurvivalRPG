using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public int Id { get; private set; } = 4;
    [SerializeField] private Camera Camera;
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
                Debug.Log("Player");
                //var correctConditions = false;
                /*foreach(var condition in ConditionsToExit)
                {
                    if(Managers.Conditions[condition])
                    {
                        correctConditions = true;
                        break;
                    }
                }
                if(correctConditions)
                {
                    if(Managers.Conditions["KILL_STRANGER"])
                    {
                        Managers.Levels.LoadScene("BadEnding");//что-то
                    }
                    if(Managers.Conditions["KILL_TREE"])
                    {
                        Managers.Levels.LoadScene("GoodEnding");//что-то
                    }
                }*/
                //moving.transform.position = NextExit.position;
                var nextPosition = NextExit.position + Offset;
                moving.SetTo(nextPosition);
                Managers.Scene.DeleteObject(moving.Id);
                Managers.Scene.SetObjectPosition(moving.Id, nextPosition);
                nextPosition.z = -10;
                Camera.transform.position = nextPosition;
            }
        }
    }
}
