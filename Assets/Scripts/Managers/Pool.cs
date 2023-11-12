using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [ReadOnly][SerializeField] private GameObject prefab;

    private List<IPoolable> allItems = new List<IPoolable>();
    private List<IPoolable> availableItems = new List<IPoolable>();
    private string itemName;

    public int ActiveAmount => allItems.Count - availableItems.Count;
    public int AllItems => allItems.Count;

    public void Initialize(GameObject prefab, int startingSize, string itemName)
    {
        this.prefab = prefab;
        this.itemName = itemName;

        for (int i = 0; i < startingSize; i++)
        {
            var item = InstantiateObject();
            item.ReturnToPool();
            availableItems.Add(item);
        }
    }

    public IPoolable Spawn()
    {
        IPoolable item = null;
        if(availableItems.Count > 0)
        {
            item = availableItems[0];
            availableItems.RemoveAt(0);
        }
        else
        {
            item = InstantiateObject();
        }
        return item;
    }

    public void BackToPool(IPoolable item)
    {
        if (allItems.Contains(item))
        {
            availableItems.Add(item);
            item.ReturnToPool();
        }
    }

    private IPoolable InstantiateObject()
    {
        var item = Instantiate(prefab).GetComponent<IPoolable>();
        item.gameObject.name = $"{itemName}_({allItems.Count})";
        item.gameObject.transform.SetParent(gameObject.transform);
        item.Initialize();
        allItems.Add(item);
        return item;
    }
}
