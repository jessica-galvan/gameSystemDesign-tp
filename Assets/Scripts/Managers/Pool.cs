using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>  where T : MonoBehaviour, IPoolable
{
    [ReadOnly][SerializeField] private T prefab;
    private Transform parent;

    private List<T> allItems = new List<T>();
    private List<T> availableItems = new List<T>();
    private string itemName;
    private int startingSize;


    public int ActiveAmount => allItems.Count - availableItems.Count;
    public int AllItems => allItems.Count;

    public Action<T, Pool<T>> OnCreate;


    public void Initialize(T prefab,  int startingSize, string itemName, Transform parent = null, Action<T, Pool<T>> OnCreate = null, bool initStartingSize = true)
    {
        this.prefab = prefab;
        this.itemName = itemName;
        this.OnCreate = OnCreate;
        this.parent = parent;
        this.startingSize = startingSize;

        if (initStartingSize)
            InstantiateSartingSize();
    }

    public void InstantiateSartingSize()
    {
        for (int i = 0; i < startingSize; i++)
        {
            var item = InstantiateObject();
            item.ReturnToPool();
            availableItems.Add(item);
        }
    }

    public T Spawn()
    {
        T item = null;
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

    public void BackToPool(T item)
    {
        if (allItems.Contains(item))
        {
            availableItems.Add(item);
            item.ReturnToPool();
        }
    }

    private T InstantiateObject()
    {
        var item = GameObject.Instantiate(prefab).GetComponent<T>();
        item.gameObject.name = $"{itemName}_({allItems.Count})";
        item.gameObject.transform.SetParent(parent);
        item.Initialize();
        allItems.Add(item);

        if(OnCreate != null)
            OnCreate(item, this);

        return item;
    }
}
