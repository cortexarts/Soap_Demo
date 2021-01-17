using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirt : MonoBehaviour
{
    [SerializeField]
    private ChemicalContent reactsWithChemical;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("YEETED " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Bullet") && reactsWithChemical == collision.gameObject.GetComponent<BulletController>().chemicalType)
        {
            Destroy(gameObject);
        }
    }
}
