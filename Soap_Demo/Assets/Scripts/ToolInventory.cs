using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInventory : MonoBehaviour
{
    public enum EquippedTool
    {
        HAND,
        SUPERSOAKER,
        SCANNER
    }

    public bool pickedUpSuperSoaker = false;

    public EquippedTool currentlyEquipped = EquippedTool.HAND;

    void Start()
    {

    }


    void Update()
    {
        SwitchBetweenTools();

    }

    void SwitchBetweenTools()
    {
        if (Input.GetAxis("Select1") > 0) //WTF IS THIS
        {
            currentlyEquipped = EquippedTool.HAND;
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        else if (Input.GetAxis("Select2") > 0)
        {
            if (pickedUpSuperSoaker)
            {
                currentlyEquipped = EquippedTool.SUPERSOAKER;
                transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true); //TODO change this 
            }
        }
    }
}
