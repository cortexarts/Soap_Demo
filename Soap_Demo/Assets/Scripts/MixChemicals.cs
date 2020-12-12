using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixChemicals : MonoBehaviour
{
    [SerializeField]
    private int nInputs = 3;
    [SerializeField]
    private PickableChemical[] inputObjects;
    [SerializeField]
    private MixedChemical[] mixedChemicalsOptions;

    public void CheckMixableChemicals()
    {
        Debug.Log("MIX MIX MIX");
        inputObjects = new PickableChemical[nInputs];
        inputObjects = RetrieveObjectsFromBases();

        
        for (int mixedChemicalOption = 0; mixedChemicalOption < mixedChemicalsOptions.Length; ++mixedChemicalOption)
        {
            bool[] gotAllChemicals = new bool[mixedChemicalsOptions[mixedChemicalOption].requiredObjects.Length];
            for (int gotChemical = 0; gotChemical < gotAllChemicals.Length; ++gotChemical)
            {
                for(int inputObject = 0; inputObject < inputObjects.Length; ++ inputObject)
                {


                    if(inputObjects[inputObject] != null && (mixedChemicalsOptions[mixedChemicalOption].requiredObjects[gotChemical].prefabName == inputObjects[inputObject].content.prefabName))
                    {
                        gotAllChemicals[gotChemical] = true;
                        break;
                    }
                    else
                    {
                        gotAllChemicals[gotChemical] = false;
                    }
                }
                
            }

            Debug.Log("Got all chemicals length" + gotAllChemicals.Length);
            bool gotAll = false;
            for(int i = 0; i < gotAllChemicals.Length; ++i)
            {
                Debug.Log($"Got chemical nr {i} : {gotAllChemicals[i]}");
                if(!gotAllChemicals[i])
                {
                    break;
                }
                if(i == gotAllChemicals.Length-1)
                {
                    gotAll = true;
                }
            }
            if(gotAll)
            {
                Debug.Log("I GOT ALL YESS");
                if(GameObject.Find("MixedChemicalSpawner").transform.childCount == 0)
                {
                    GameObject.Instantiate(mixedChemicalsOptions[mixedChemicalOption].ObjectPrefab, GameObject.Find("MixedChemicalSpawner").transform.position+(Vector3.up), GameObject.Find("MixedChemicalSpawner").transform.rotation);
                }
            }
        }         

        foreach(PickableChemical pObject in inputObjects)
        {
            //Check all chems for each in the mixed options

            if(pObject)
            {
                Debug.Log("I contain:  " + pObject.content.prefabName);
            }
            else
            {
                Debug.Log("I contain nothing");
            }
            
        }
    }

    public PickableChemical[] RetrieveObjectsFromBases()
    {
        PickableChemical[] objects = new PickableChemical[nInputs];
        Debug.Log("I have n many childs" + transform.childCount);
        for (int i = 0; i < transform.childCount; ++i) 
        {
            if(transform.GetChild(i).CompareTag("MixerInput"))
            { 
                if(transform.GetChild(i).Find("Hover").GetComponent<HoldChemical>().heldItem != null)
                {
                    Debug.Log("Adding" + transform.GetChild(i).Find("Hover").GetComponent<HoldChemical>().heldItem + "To list");
                    objects[i] = transform.GetChild(i).Find("Hover").GetComponent<HoldChemical>().heldItem;
                }
            }
        }
        return objects;
    }
}
