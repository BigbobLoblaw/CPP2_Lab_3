using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    CharacterController controller;

    // Attributes will help organize the Inspector
    [Header("Player Settings")]
    [Space(2)]
    [Tooltip("Speed value between 1 and 10.")]
    [Range(1.0f, 15.0f)]
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    public float gravity;

    Vector3 moveDirection;

    enum ControllerType { SimpleMove, Move };
    [SerializeField] ControllerType type;

    [Header("Weapon Settings")]
    // Handle weapon shooting
    public float projectileForce;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("Raycast Settings")]
    // Raycast variables
    public Transform thingToLookFrom;
    public float lookAtDistance;

    public GameObject BooPrefab;

    // Start is called before the first frame update
    void Start()
    {

        try
        {
            controller = GetComponent<CharacterController>();

            controller.minMoveDistance = 0.0f;

            if (speed <= 0)
            {
                speed = 6.0f;

                //   Debug.Log("Speed not set on " + name + " defaulting to " + speed);
            }

            if (jumpSpeed <= 0)
            {
                jumpSpeed = 6.0f;

                //   Debug.Log("JumpSpeed not set on " + name + " defaulting to " + jumpSpeed);
            }

            if (rotationSpeed <= 0)
            {
                rotationSpeed = 10.0f;

                //  Debug.Log("RotationSpeed not set on " + name + " defaulting to " + rotationSpeed);
            }

            if (gravity <= 0)
            {
                gravity = 9.81f;

                //    Debug.Log("Gravity not set on " + name + " defaulting to " + gravity);
            }

            moveDirection = Vector3.zero;

            if (projectileForce <= 0)
            {
                projectileForce = 15.0f;

                //   Debug.Log("ProjectileForce not set on " + name + " defaulting to " + projectileForce);
            }

            if (!projectilePrefab)
                //   Debug.LogWarning("Missing projectilePrefab on " + name);

                if (!projectileSpawnPoint)
                    //  Debug.LogWarning("Missing projectileSpawnPoint on " + name);

                    if (lookAtDistance <= 0)
                    {
                        lookAtDistance = 100.0f;
                        // Debug.Log("LookAtDistance not set on " + name + " defaulting to " + lookAtDistance);
                    }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            //  Debug.LogWarning("Always get called");
        }


    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ControllerType.SimpleMove:

                //transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

                controller.SimpleMove(transform.forward * Input.GetAxis("Vertical") * speed);

                break;

            case ControllerType.Move:

                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection *= speed;

                    moveDirection = transform.TransformDirection(moveDirection);

                    if (Input.GetButtonDown("Jump"))
                        moveDirection.y = jumpSpeed;
                }

                moveDirection.y -= gravity * Time.deltaTime;

                controller.Move(moveDirection * Time.deltaTime);

                break;
        }

        // Usage Raycast
        // - GameObject needs a Collider
        RaycastHit hit;

        if (!thingToLookFrom)
        {
            Debug.DrawRay(transform.position, transform.forward * lookAtDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, lookAtDistance))
            {
              
            }
        }
        else
        {
            Debug.DrawRay(thingToLookFrom.transform.position, thingToLookFrom.transform.forward * lookAtDistance, Color.yellow);

            if (Physics.Raycast(thingToLookFrom.transform.position, thingToLookFrom.transform.forward, out hit, lookAtDistance))
            {
                //Debug.Log("Raycast hit: " + hit.transform.name);

                if (hit.collider.tag == "boo")
                {
                    BooPrefab.SetActive(false);
                    BooPrefab.GetComponent<FollowPlayer>().isShooting = false;
                    //  Debug.Log("booisinvisable");

                }

                else
                {
                    BooPrefab.SetActive(true);
                    //Debug.Log("booisvisable");

                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
            Fire();

    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("HIT");
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
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
            Destroy(temp.gameObject, 2.0f);
        }
    }



    // Adds a menu option to reset stats
    [ContextMenu("Reset Stats")]
    void ResetStats()
    {
        //Debug.Log("Perform operation");
        speed = 6.0f;
    }
}
