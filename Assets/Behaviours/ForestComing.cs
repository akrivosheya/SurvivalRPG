using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestComing : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Direction;
    [SerializeField] private int Mode;
    [SerializeField] private int Prod;
    private Vector3 _firstPosition;

    void Start()
    {
        _firstPosition = transform.position;
    }

    void LateUpdate()
    {
        Debug.Log(Target.position + " " +  _firstPosition);
        float magnitude;
        if(Mode == 0)
        {
            magnitude = Prod/Mathf.Abs(Target.position.y - _firstPosition.y);
        }
        else
        {
            magnitude = Prod/Mathf.Abs(Target.position.x - _firstPosition.x);
        }
        transform.position = Direction * magnitude + _firstPosition;
    }
}
