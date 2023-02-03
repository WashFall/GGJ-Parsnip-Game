using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterControls characterControls;
    private Rigidbody rigidBody;
    private Vector3 forward, right;

    public float moveSpeed = 10;
    public float rotateSpeed = 20;

    void Start()
    {
        characterControls = new CharacterControls();
        characterControls.Enable();

        rigidBody = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 rightMovement = right * characterControls.Movement.Horizontal.ReadValue<float>();
        Vector3 upMovement = forward * characterControls.Movement.Vertical.ReadValue<float>();

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        Vector3 newPosition = new Vector3();
        newPosition += rightMovement;
        newPosition += upMovement;
        if (rightMovement.magnitude > 0 || upMovement.magnitude > 0)
            transform.forward = Vector3.Lerp(transform.forward, heading, rotateSpeed * Time.deltaTime);

        rigidBody.velocity = newPosition.normalized * moveSpeed;
    }
}
