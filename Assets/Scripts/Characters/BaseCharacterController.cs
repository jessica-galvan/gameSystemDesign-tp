using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour, IUpdate
{

    //TODO RETHINK WHEN ADDING POOL
    public virtual void Initialize()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    public virtual bool CanUpdate()
    {
        return GameManager.Instance.Pause || GameManager.Instance.Won; //!model.Alive
    }

    public abstract void Refresh();
    protected abstract void AddToUpdate();
    protected abstract void RemoveFromUpdate();
}
