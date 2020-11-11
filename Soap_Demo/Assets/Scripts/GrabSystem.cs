﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField]
    private Transform slot;
    private PickableObject pickedItem = null;

    public GameObject lastHover;

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

    private void Start()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Chemical"))
        {
            Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponentInParent<Collider>());
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

        RaycastHit placement;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out placement, grabDistance))
        {
            if (placement.collider.CompareTag("Hover"))
            {
                if (pickedItem)
                {
                    placement.collider.GetComponent<MeshRenderer>().enabled = true;
                    lastHover = placement.collider.gameObject;
                    if (Input.GetButtonDown("Fire1"))
                    {
                        PlaceItem(pickedItem, placement.collider.gameObject);
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Retrieveitem(placement.collider.gameObject);
                    }
                }
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                if (pickedItem)
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
            else if(lastHover != null)
            {
                lastHover.GetComponent<MeshRenderer>().enabled = false;
                lastHover = null;
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
            item.transform.SetParent(null);
            item.transform.position = targetHover.transform.position;
            targetHover.GetComponent<MeshRenderer>().enabled = false;
            //targetHover.GetComponent<CapsuleCollider>().enabled = false;
            HoldChemical hold = targetHover.GetComponent<HoldChemical>();

            hold.heldItem = item;
        }

        void Retrieveitem(GameObject targetHover)
        {
            HoldChemical hold = targetHover.GetComponent<HoldChemical>();
            if (hold.heldItem)
            {
                PickItem(hold.heldItem);

            }
            else
            {
                Debug.LogError("Can't pick item from target hover");
            }
        }
    }

