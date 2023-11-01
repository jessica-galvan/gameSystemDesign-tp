using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectableOption 
{
    string Title { get; }
    Sprite Icon { get; }
    string GetDescription();
    bool IsPowerUp();
}
