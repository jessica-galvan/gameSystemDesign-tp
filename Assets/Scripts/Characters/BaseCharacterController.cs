using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController<T> : MonoBehaviour, IUpdate where T: BaseCharacterModel
{
    public T Model { get; private set; }

    //TODO RETHINK WHEN ADDING POOL
    public virtual void Initialize()
    {
        Model = GetComponent<T>();
        Model.Initialize();
        AddToUpdate();
    }

    public virtual bool CanUpdate()
    {
        return GameManager.Instance.CanUpdate; //&& Model.Alive;
    }

    public abstract void Refresh();
    protected abstract void AddToUpdate();
    protected abstract void RemoveFromUpdate();
}
