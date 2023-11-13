using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController<T> : MonoBehaviour, IUpdate, IFixedUpdate where T: BaseCharacterModel
{
    [SerializeField] protected CharacterDataSO stats;
    public T Model { get; private set; }
    public CharacterDataSO Stats => stats;

    public virtual void Initialize()
    {
        Model = GetComponent<T>();
        Model.Initialize(stats);
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
