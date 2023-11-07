using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController<T> : MonoBehaviour, IUpdate, IFixedUpdate where T: BaseCharacterModel
{
    public T Model { get; private set; }

    public virtual void Initialize()
    {
        Model = GetComponent<T>();
        Model.Initialize();
    }

    public virtual bool CanUpdate()
    {
        return GameManager.Instance.CanUpdate; //&& Model.Alive;
    }

    public abstract void Refresh(float deltaTime);
    public abstract void FixedRefresh();
    protected abstract void AddToUpdate();
    protected abstract void RemoveFromUpdate();
}
