using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSoakerController : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletVelocity = 10;

    [SerializeField]
    private float firerate = 50;

    private GameObject instantiadBullet;

    private float time;

    [SerializeField]
    private ChemicalContent loadedChemical;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        FireBullet();
    }

    void FireBullet()
    {
        if (Input.GetMouseButton(0) && Time.time - time > 60 / firerate)
        {
            Vector3 position = transform.GetChild(0).transform.position;
            Quaternion rotation = transform.GetChild(0).transform.rotation;
            instantiadBullet = Instantiate<GameObject>(bullet, position, rotation);
            instantiadBullet.GetComponent<BulletController>().SetChemicalType(loadedChemical);
            Vector3 bulletTrajectory = transform.GetChild(0).transform.forward * bulletVelocity;
            instantiadBullet.GetComponent<Rigidbody>().AddForce(bulletTrajectory, ForceMode.Impulse);
            Destroy(instantiadBullet, 3);
            time = Time.time;
        }
    }
}
