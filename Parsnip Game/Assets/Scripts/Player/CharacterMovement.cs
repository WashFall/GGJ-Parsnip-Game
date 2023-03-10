using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector3 forward, right;
    private InputManager inputManager;

    public float moveSpeed;
    public float rotateSpeed;
    public Camera playerCamera;
    public bool canMove;

    void Start()
    {
        canMove = true;
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();

        forward = playerCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    private void FixedUpdate()
    {
        if(canMove) { Move(); }
    }

    private void Move()
    {
        Vector3 rightMovement = right * inputManager.horizontal.ReadValue<float>();
        Vector3 upMovement = forward * inputManager.vertical.ReadValue<float>();

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        Vector3 newPosition = new Vector3();
        newPosition += rightMovement;
        newPosition += upMovement;

        float rotateStep = rotateSpeed * Time.fixedDeltaTime;

        if (rightMovement.magnitude > 0 || upMovement.magnitude > 0)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, heading, rotateStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            rigidBody.velocity = newPosition.normalized * moveSpeed;
        }
        else
        {
            if(rigidBody.velocity.magnitude > 0)
            {
                rigidBody.velocity *= 0.9f;
            }

            if (rigidBody.velocity.magnitude < 0.01f) rigidBody.velocity = Vector3.zero;
        }

    }
}
