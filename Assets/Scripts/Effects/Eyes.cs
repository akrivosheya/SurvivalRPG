using UnityEngine;

public class Eyes : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float LimitDistance;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();//проверка
    }

    void LateUpdate()
    {
        var fixedPosition = transform.position;
        fixedPosition.z = Target.position.z;
        var distance = (Target.position - fixedPosition).magnitude;
        if(distance <= LimitDistance)
        {
            _animator.SetBool("HIDING", true);//заменить
        }
        else
        {
            _animator.SetBool("HIDING", false);
        }
    }
}
