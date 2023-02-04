using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public float health = 100f;

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
