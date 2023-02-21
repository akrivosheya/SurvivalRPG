using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float CellLength = 1f;
    [SerializeField] private float PositionRate = 0.2f;
    private Vector3 _nextPosition;
    private Vector3 _movement;
    private bool _isMoving = false;

    void Start()
    {
    }

    void Update()
    {
        if(_isMoving)
        {
            transform.Translate(_movement * Time.deltaTime * Speed);
            if((transform.position - _nextPosition).magnitude < PositionRate)
            {
                _isMoving = false;
                Debug.Log("stop moving");
            }
        }
        else
        {

            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");
            if(Mathf.Approximately(xAxis, 0) && Mathf.Approximately(yAxis, 0))
            {
                return;
            }
            var directionX = (int)((Mathf.Approximately(xAxis, 0)) ? 0 : Mathf.Sign(xAxis));
            var directionY = (int)((Mathf.Approximately(yAxis, 0)) ? 0 : Mathf.Sign(yAxis));
            Managers.Scene.FixDirection(ref directionX, ref directionY);
            if(directionX == 0 && directionY == 0)
            {
                return;
            }
            _movement = new Vector3(directionX, directionY, 0);
            _movement.Normalize();
            _isMoving = true;
            _nextPosition = new Vector3(transform.position.x + CellLength * directionX,
                transform.position.y + CellLength * directionY, 0);
            Debug.Log("start moving");
        }
    }
}
