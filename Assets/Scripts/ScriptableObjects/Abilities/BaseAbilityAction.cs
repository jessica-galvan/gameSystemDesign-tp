using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityAction : ScriptableObject
{
    public abstract void Initialize();
    public abstract void Execute(BaseCharacterModel characterModel);
}
