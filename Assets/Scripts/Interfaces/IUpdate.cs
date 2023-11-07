using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate 
{
    void Refresh(float deltaTime);
}

public interface IFixedUpdate
{
    void FixedRefresh();
}
