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
    private List<BuildingHealth> buildingHealth;

    public ParticleSystem scatterFx;
    public delegate void Explosion();
    public static Explosion explosion;
    public GameObject explosionSite;
    public Transform currentExplosionSite;
    public GameObject root;

    [SerializeField] private SphereCollider maxRadius;
    [SerializeField] private Rigidbody rb;

    private bool prematureAttack;
    private bool hasAttackedRecently;

    private float attackProgress;
    private float damage;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (inputManager.attack.ReadValue<float>() == 1)
            StartAttack();

        Debug.Log(attackProgress);

        
        if (attackProgress >= 1 && !hasAttackedRecently)
        {
            hasAttackedRecently = true;
            scatterFx.Emit(5000);
            ExplosionDamage(transform.position, maxRadius.radius);
            explosion?.Invoke();
            GameObject newExplosion = Instantiate(explosionSite, transform.position, Quaternion.identity);
            currentExplosionSite = newExplosion.transform;
            StartCoroutine(Stuck());
        }


        damage = attackProgress * 100f;
    }

    private void StartAttack()
    {
        if (hasAttackedRecently) return;

        rb.velocity = Vector3.zero;
        if (attackProgress < 1 && !hasAttackedRecently) attackProgress += 0.5f * Time.deltaTime;

        if (attackProgress > 1)
        {
            attackProgress = 1;
        }
    }

    private IEnumerator Stuck()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(1f);
        rb.isKinematic = false;
        attackProgress = 0;
        hasAttackedRecently = false;
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(center, radius, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out BuildingHealth bh))
            {
                bh.DamageHealth(damage);
                Instantiate(root);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            //Debug.Log("I found a building!");
            buildingHealth.Add(other.GetComponentInChildren<BuildingHealth>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Building")
        {
            //Debug.Log("Bye building!");
            buildingHealth.Remove(other.GetComponent<BuildingHealth>());
        }
    }

}
