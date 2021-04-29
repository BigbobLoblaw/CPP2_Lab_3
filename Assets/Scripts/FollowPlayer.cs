using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public bool isShooting;

    public float moveSpeed;
    public float maxDistance;
    public float minDistance;

    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce;

    void Start()
    {
        if(moveSpeed <= 0)
        {
            moveSpeed = 4;
        }
        if (maxDistance <= 0)
        {
            maxDistance = 100;
        }
        if (minDistance <= 5)
        {
            minDistance = 0;
        }
        if (projectileForce <= 0)
        {
            projectileForce = 20;
        }

    }

    public void Fire()
    {
        Debug.Log("Pew Pew");

        if (projectileSpawnPoint && projectilePrefab)
        {
            // Make projectile
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            // Shoot projectile
            temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            // Destroy projectile after 2.0 seconds
            Destroy(temp.gameObject, 4.0f);


        }

    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if (Vector3.Distance(transform.position, player.position) >= minDistance)
        {

            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, player.position) <= maxDistance)
            {
                if(!isShooting)
                {
                    StartCoroutine(ShootWaitTime(2f));
                }
            }
        }
    }

    IEnumerator ShootWaitTime(float wait)
    {
        isShooting = true;
        yield return new WaitForSecondsRealtime(wait);

        if (isShooting == true)
        {
            Fire();
        }

        isShooting = false;

    }
}
