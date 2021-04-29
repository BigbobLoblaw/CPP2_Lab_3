using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooRayCast : MonoBehaviour
{


    public Transform booRayCast;

    public float booRayCastDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (booRayCastDistance <= 0)
        {
            booRayCastDistance = 15.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit raycastHit;

        if (!booRayCast)
        {
            Debug.DrawRay(transform.position, transform.forward * booRayCastDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, booRayCastDistance))
            {

            }
        }
        else
        {
            Debug.DrawRay(booRayCast.transform.position, booRayCast.transform.forward * booRayCastDistance, Color.yellow);

            if (Physics.Raycast(booRayCast.transform.position, booRayCast.transform.forward, out raycastHit, booRayCastDistance))
            {
                

            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("HIT");
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        
    }
}