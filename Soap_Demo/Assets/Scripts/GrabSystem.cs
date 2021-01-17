using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField]
    private Transform slot;
    private PickableObject holdingItem = null;

    private GameObject lastHover = null;

    [SerializeField]
    private float grabDistance = 2f;

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

    void Update()
    {
        if (GetComponentInParent<ToolInventory>().currentlyEquipped == ToolInventory.EquippedTool.HAND)
        {
            HandleObject();
        }

    }

    void HandleObject()
    {
        Ray grabRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        bool hasRaycastHit = Physics.Raycast(grabRay, out RaycastHit raycastHit, grabDistance);

        if (hasRaycastHit)
        {
            m_Crosshair.SetCrosshair(CrosshairType.Hover);
        }
        else
        {
            m_Crosshair.SetCrosshair(CrosshairType.Default);
        }

        //has raycast hit   raycastHit.collider.gameObject.GetComponent<PickableObject>().objectType == PickableObject.ObjectType.SUPERSOAKER
        if (hasRaycastHit && raycastHit.collider && holdingItem == null)
        {
            //Debug.Log("Handling super soaker");
            if (raycastHit.collider.gameObject.GetComponent<PickableObject>())
            {
                if (raycastHit.collider.gameObject.GetComponent<PickableObject>().objectType == PickableObject.ObjectType.SUPERSOAKER) //Null reference
                {
                    HandleSuperSoaker(raycastHit);
                }
                else if (raycastHit.collider.gameObject.GetComponent<PickableObject>().objectType == PickableObject.ObjectType.CHEMICAL)
                {
                    HandleChemical(raycastHit, hasRaycastHit);
                }
            }
            else
            {
                HandleChemical(raycastHit, hasRaycastHit);
            }
        }
        else
        {
            //Debug.Log("Handling chemical");
            HandleChemical(raycastHit, hasRaycastHit);
        }
    }

    void HandleSuperSoaker(RaycastHit raycastHit)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true); //TODO change this 
            Destroy(raycastHit.collider.gameObject);
            GetComponentInParent<ToolInventory>().pickedUpSuperSoaker = true;
            GetComponentInParent<ToolInventory>().currentlyEquipped = ToolInventory.EquippedTool.SUPERSOAKER;
        }
    }

    void HandleChemical(RaycastHit raycastHit, bool hasRaycastHit)
    {
        bool triedPlacing = false;

        if (lastHover)
        {
            if (lastHover.CompareTag("Hover") && (!hasRaycastHit || (hasRaycastHit && !raycastHit.collider.CompareTag("Hover"))))
            {
                lastHover.GetComponent<MeshRenderer>().enabled = false;
                lastHover = null;
            }
        }

        if (hasRaycastHit && raycastHit.collider.CompareTag("Hover"))
        {
            triedPlacing = true;
            if (holdingItem)
            {
                if (raycastHit.collider.GetComponent<HoldChemical>().heldItem == null)
                {
                    raycastHit.collider.GetComponent<MeshRenderer>().enabled = true;
                }
                lastHover = raycastHit.collider.gameObject;
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("Trying to place chemical");
                    PlaceItem(holdingItem, raycastHit.collider.gameObject);
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("Trying to retrieve chemical");
                    Retrieveitem(raycastHit.collider.gameObject);
                }
            }
        }
        if (Input.GetButtonDown("Fire1") && !triedPlacing)
        {
            Debug.Log($"Pickeditem: {holdingItem}");
            if (holdingItem)
            {
                Debug.Log("Trying to drop up chemical");
                DropItem(holdingItem);
            }
            else
            {
                if (hasRaycastHit)
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastHit.distance, Color.yellow);
                    var pickable = raycastHit.transform.GetComponent<PickableObject>();
                    if (pickable)
                    {
                        //Debug.Log("Trying to pick up chemical");
                        PickItem(pickable);
                    }
                }
            }
        }
    }

    void PickItem(PickableObject item)
    {
        holdingItem = item;
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
        holdingItem = null;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.detectCollisions = true;
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }

    void PlaceItem(PickableObject item, GameObject targetHover)
    {
        Debug.Log(targetHover.GetComponent<HoldChemical>().heldItem);
        if (targetHover.GetComponent<HoldChemical>().heldItem == null)
        {
            holdingItem = null;
            item.transform.SetParent(null);
            item.transform.position = targetHover.transform.position;
            item.transform.rotation = targetHover.transform.rotation;
            targetHover.GetComponent<MeshRenderer>().enabled = false;
            //targetHover.GetComponent<CapsuleCollider>().enabled = false;
            HoldChemical hold = targetHover.GetComponent<HoldChemical>();

            hold.heldItem = (PickableChemical)item;
        }
    }

    void Retrieveitem(GameObject targetHover)
    {
        HoldChemical hold = targetHover.GetComponent<HoldChemical>();
        if (hold.heldItem)
        {
            PickItem(hold.heldItem);
            hold.heldItem = null;
        }
        else
        {
            Debug.LogError("Can't pick item from target hover");
        }
    }
}

