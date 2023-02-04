using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputManager))]

public class Attack : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterMovement characterMovement;
    
    public ParticleSystem scatterFx;


    private SphereCollider attackSphere;
    private SphereCollider maxRadius;
    private Rigidbody rb;

    private float attackProgress;
    private float attackRadius;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        characterMovement = GetComponent<CharacterMovement>();
        
        attackSphere = GetComponent<SphereCollider>();
        maxRadius = transform.GetChild(2).GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (inputManager.attack.ReadValue<float>() == 1)
            DoAttack();
        else
            ReleaseAttack();
        

        if (attackProgress >= 1)
            scatterFx.Emit(5000);

        float i = attackProgress * maxRadius.radius;
        attackSphere.radius = i;

        if (attackProgress == 0)
        {
            characterMovement.canMove = true;
        }
    }

    private void DoAttack()
    {
        rb.velocity = Vector3.zero;
        characterMovement.canMove = false;
        if (attackProgress < 1) attackProgress += 0.5f * Time.deltaTime;
        if (attackProgress > 1) attackProgress = 1;
    }

    private void ReleaseAttack()
    {
        if (attackProgress > 0) attackProgress -= Time.deltaTime;
        if (attackProgress < 0) attackProgress = 0;
    }


}
