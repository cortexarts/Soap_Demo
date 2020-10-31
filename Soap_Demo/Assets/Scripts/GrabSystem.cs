using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField]
    private Transform slot;
    private PickableObject pickedItem = null;

    private GameObject lastHover;

    public float grabDistance = 2f;

    private void Start()
    {
        foreach(var item in GameObject.FindGameObjectsWithTag("Chemical"))
        {
            Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponentInParent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit placement;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out placement, grabDistance))
        {
            if (placement.collider.CompareTag("Hover") && pickedItem)
            {
                placement.collider.GetComponent<MeshRenderer>().enabled = true;
                lastHover = placement.collider.gameObject;
            }
        }
        else
        {
            if(lastHover)
            {
                lastHover.GetComponent<MeshRenderer>().enabled = false;
                lastHover = null;
            }
        }


        if (Input.GetButtonDown("Fire1"))
        {
            if (placement.collider.CompareTag("Hover") && pickedItem)
            {
                PlaceItem(pickedItem, placement.collider.gameObject);
            }
            else if (pickedItem)
            {
                DropItem(pickedItem);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabDistance))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    var pickable = hit.transform.GetComponent<PickableObject>();
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

    void PlaceItem(PickableObject item, GameObject targetHover)
    {
        pickedItem = null;
        item.transform.SetParent(targetHover.transform);
        item.transform.position = targetHover.transform.position;
        targetHover.GetComponent<MeshRenderer>().enabled = false;
        targetHover.GetComponent<CapsuleCollider>().enabled = false;
    }
}
