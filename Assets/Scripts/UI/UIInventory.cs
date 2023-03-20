using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private List<UIItem> Items;//привязка id в Inventory
    [SerializeField] private List<Text> Counts;

    void Awake()
    {
        Messenger.AddListener(GameEvent.ITEM_ADDED, OnItemAdded);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ITEM_ADDED, OnItemAdded);
    }

    void Start()
    {
        Clear();
    }

    public void OnItemAdded()
    {
        Clear();
        foreach(var id in Managers.Inventory)
        {
            if(id == 0)
            {
                continue;
            }
            Items[id - 1].gameObject.SetActive(true);
            var count = Managers.Inventory.GetCount(id);
            if(count > 1)
            {
                Counts[id - 1].gameObject.SetActive(true);
                Counts[id - 1].text = count.ToString();
            }
        }
    }

    private void Clear()
    {
        for(int i = 0; i < Items.Count; ++i)
        {
            Items[i].gameObject.SetActive(false);
            Counts[i].gameObject.SetActive(false);
        }
    }
}
