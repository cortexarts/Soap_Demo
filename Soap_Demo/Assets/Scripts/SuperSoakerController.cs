using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSoakerController : MonoBehaviour
{

    public GameObject bullet;
    private GameObject instantiadBullet;
    public float bulletVelocity;
    public float firerate;
    private bool firing = false;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButton(0) && Time.time - time > 60/firerate)
        {
            
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            instantiadBullet = Instantiate<GameObject>(bullet, position, rotation);
            Vector3 bulletTrajectory = transform.forward * bulletVelocity;
            instantiadBullet.GetComponent<Rigidbody>().AddForce(bulletTrajectory, ForceMode.Impulse);
            Destroy(instantiadBullet, 3);
            time = Time.time;
        }
    }
}
