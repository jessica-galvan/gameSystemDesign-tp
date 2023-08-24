using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabsReferences", menuName = "TP/PrefabsReferences", order = 4)]
public class PrefabsReferencesSO : ScriptableObject
{
    public UpdateManager updateManagerPrefab;
    public PlayerController playerPrefab;
    //TODO make death prefab and pool 
}
