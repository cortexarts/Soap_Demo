using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MixedChemical", menuName = "ScriptableObjects/MixedChemical", order = 1)]
public class MixedChemical : ScriptableObject
{
    public string prefabName;
    public GameObject ObjectPrefab;
    public ChemicalContent[] requiredObjects;
    //TODO Add attributes
}