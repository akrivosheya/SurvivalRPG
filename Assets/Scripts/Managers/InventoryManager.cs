using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public string NewItem { get; private set; }
    [SerializeField] private List<string> Items;
    private Dictionary<int, int> _items = new Dictionary<int, int>();

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void AddItem(int itemId)
    {
        if(!_items.ContainsKey(itemId))
        {
            _items.Add(itemId, 0);
        }
        _items[itemId]++;
        NewItem = Items[itemId];//проверка
    }

    public int GetCount(int itemId)
    {
        if(!_items.ContainsKey(itemId))
        {
            return 0;
        }
        return _items[itemId];
    }
}
