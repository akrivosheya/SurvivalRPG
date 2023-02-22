using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour, Item
{
    public int Id { get; private set; } = 3;
    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
    }
}
