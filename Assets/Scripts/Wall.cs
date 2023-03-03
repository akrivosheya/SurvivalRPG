using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int Id { get; private set; } = 1;

    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
    }
}
