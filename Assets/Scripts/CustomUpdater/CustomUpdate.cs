using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUpdate : MonoBehaviour
{
    [SerializeField] [ReadOnly] public string updaterName;
    private List<IUpdate> updatingList = new List<IUpdate>();
    private float targetTime;
    private float currentTime;
    private bool limitTargetFrame;
    private float deltaTime;

    public float DeltaTime => deltaTime;

    public void Initialize(int targetFrame, string displayName = "")
    {
        updaterName = displayName;

        limitTargetFrame = targetFrame > 0;
        if (limitTargetFrame)
            targetTime = 1f / targetFrame; //calculamos el tiempo de cada framerate
    }

    public void UpdateList()
    {
        deltaTime += Time.deltaTime;

        //en cada frame, nos fijamos si es el momento de updatear esta lista, si devuelve falso, no updatea y ya. 
        if (limitTargetFrame && !CanUpdate()) return;

        for (int i = 0; i < updatingList.Count; i++)
            updatingList[i].Refresh(deltaTime);

        deltaTime = 0f;
    }

    private bool CanUpdate()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = targetTime;
            return true;
        }
        return false;
    }

    public void Add(IUpdate item)
    {
        if (!updatingList.Contains(item))
            updatingList.Add(item);
    }

    public void Remove(IUpdate item)
    {
        if (updatingList.Contains(item))
            updatingList.Remove(item);
    }
}
