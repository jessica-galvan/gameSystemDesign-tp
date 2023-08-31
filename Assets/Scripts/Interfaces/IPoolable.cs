using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    GameObject gameObject { get; }
    void Initialize();
    void ReturnToPool();
}
