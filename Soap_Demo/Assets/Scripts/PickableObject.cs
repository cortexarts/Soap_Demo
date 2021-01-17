using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{

    private Rigidbody rb;
    public Rigidbody Rb => rb;

    public enum ObjectType
    {
        CHEMICAL,
        SUPERSOAKER
    }

    public ObjectType objectType;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
