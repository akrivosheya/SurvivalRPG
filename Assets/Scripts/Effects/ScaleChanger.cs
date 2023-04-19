using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Transform ObjectToScale;
    [SerializeField] private int Prod;
    private Vector3 _firstPosition;
    private Vector3 _firstScale;

    void Start()
    {
        _firstPosition = transform.position;
        _firstScale = ObjectToScale.transform.localScale;

    }

    void LateUpdate()
    {
        float magnitude;
        magnitude = Prod/Mathf.Abs(Target.position.y - _firstPosition.y);
        var scaleChanging = new Vector3(0, magnitude, 0);
        ObjectToScale.transform.localScale = _firstScale + scaleChanging;
    }
}
