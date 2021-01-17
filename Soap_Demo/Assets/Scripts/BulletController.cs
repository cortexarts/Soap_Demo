using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public ChemicalContent chemicalType;

    [SerializeField]
    private GameObject splash;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Splash();
            Destroy(gameObject);
        }
    }

    void Splash()
    {
        Quaternion quaternion = Quaternion.identity;
        quaternion = Quaternion.AngleAxis(-90f, Vector3.right);
        GameObject sploosh = Instantiate(splash, transform.position, quaternion);
        sploosh.GetComponent<ParticleSystem>().Play();
        Destroy(sploosh, 2);
    }

    public void SetChemicalType(ChemicalContent newChemicalContent)
    {
        chemicalType = newChemicalContent;

    }
}
