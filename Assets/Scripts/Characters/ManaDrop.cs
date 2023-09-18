using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : MonoBehaviour, ICollectable
{
    public int manaAmount = 5;

    public void PickUp()
    {
        GameManager.Instance.manaSystem.AddMana(manaAmount);
        //TODO ADD TO POOL!!!!
        Destroy(gameObject);
    }
    //TODO ADD LIFE TIMER
}
