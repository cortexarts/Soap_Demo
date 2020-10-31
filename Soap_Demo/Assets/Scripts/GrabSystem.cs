using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField]
    private Transform slot;
    private PickableObject pickedItem = null;

    public float grabDistance = 2f;

    private Crosshair m_Crosshair;

    private void Awake()
    {
        m_Crosshair = GetComponent<Crosshair>();
        if (m_Crosshair == null)
        {
            Debug.LogError("Failed to get Crosshair");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        bool hasRaycastHist = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastHit, grabDistance);
        if (hasRaycastHist)
        {
            m_Crosshair.SetCrosshair(CrosshairType.Hover);
        }
        else
        {
            m_Crosshair.SetCrosshair(CrosshairType.Default);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if(pickedItem)
            {
                DropItem(pickedItem);
            }
            else
            {
                if (hasRaycastHist)
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastHit.distance, Color.yellow);
                    var pickable = raycastHit.transform.GetComponent<PickableObject>();
                    if (pickable)
                    {
                        Debug.Log("Trying to pick up chemical");
                        PickItem(pickable);

                    }
                }
            }
        }
    }


    void PickItem(PickableObject item)
    {
        pickedItem = item;
        if(item == null)
        {
            Debug.Log("REEEEEEEEEEEEEEE");
        }

        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;
        item.Rb.detectCollisions = false;

        item.transform.SetParent(slot);

        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

    void DropItem(PickableObject item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.detectCollisions = true;
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }
}
