using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private Dictionary<string, bool> _conditions;

    public void Startup()
    {
        _conditions = new Dictionary<string, bool>();
        status = ManagerStatus.Started;
    }

    public void AddCondition(string condition)
    {
        _conditions[condition] = true;
    }

    public void DeleteCondition(string condition)
    {
        _conditions[condition] = false;
    }

    public bool this[string key]
    {
        get
        {
            if(_conditions.ContainsKey(key))
            {
                return _conditions[key];
            }
            else
            {
                return false;
            }
        }
    }
}
