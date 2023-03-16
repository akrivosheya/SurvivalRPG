using UnityEngine;

public class SpeedSetter : MonoBehaviour
{
    public float Speed { get { return _speed; } }
    [SerializeField] private float _speed;
}
