using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{

    public float interactDistance = 2f; //Change into scriptable object for all scripts

    void Update()
    {
        if (Input.GetButtonDown("Interact") && Physics.Raycast(transform.position, (transform.forward),  out RaycastHit raycastHit, interactDistance))
        {
            Debug.Log("Pressed E and hit: " + raycastHit.collider.gameObject.name);
            Debug.DrawRay(transform.position, transform.TransformDirection(transform.forward), Color.red);
            if(raycastHit.collider.gameObject.name == "Button")
            {
                raycastHit.collider.GetComponentInParent<MixChemicals>().CheckMixableChemicals();
            }

        }
    }
}
