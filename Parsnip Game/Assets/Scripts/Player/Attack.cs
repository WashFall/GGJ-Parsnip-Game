using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputManager))]

public class Attack : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterMovement characterMovement;
    private BuildingHealth buildingHealth;
    
    public ParticleSystem scatterFx;
    public delegate void Explosion();
    public static Explosion explosion;
    public GameObject explosionSite;
    public Transform currentExplosionSite;

    private SphereCollider attackSphere;
    private SphereCollider maxRadius;
    private Rigidbody rb;

    private bool prematureAttack;
    private bool hasAttackedRecently;

    private float attackProgress;
    private float damage;

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
            StartAttack();
        else
            ReleaseAttack();

        float activeRadius = attackProgress * maxRadius.radius;
        attackSphere.radius = activeRadius;
        
        if (attackProgress >= 1 && !hasAttackedRecently)
        {
            hasAttackedRecently = true;
            scatterFx.Emit(5000);
            DoDamage();
            explosion?.Invoke();
            GameObject newExplosion = Instantiate(explosionSite, transform.position, Quaternion.identity);
            currentExplosionSite = newExplosion.transform;
        }

        if (attackProgress == 0)
            characterMovement.canMove = true;

        damage = attackProgress * 100f;
    }

    private void StartAttack()
    {
        rb.velocity = Vector3.zero;
        characterMovement.canMove = false;
        if (attackProgress < 1) attackProgress += 0.5f * Time.deltaTime;

        if (attackProgress > 1)
        {
            attackProgress = 1;
        }
    }

    private void ReleaseAttack()
    {
        if (attackProgress > 0) attackProgress -= Time.deltaTime;
        if (attackProgress < 0)
        {
            hasAttackedRecently = false;
            attackProgress = 0;
        }
    }

    private void DoDamage()
    {
        if (buildingHealth == null) return;

        if (prematureAttack)
            damage *= 0.5f;

        buildingHealth.DamageHealth(damage);
        //Debug.Log("Damage: " + damage);
        //Debug.Log("We did some damage! Health left: " + buildingHealth.health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            //Debug.Log("I found a building!");
            buildingHealth = other.GetComponent<BuildingHealth>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
        {
            //Debug.Log("Bye building!");
            buildingHealth = null;
        }
    }

}
