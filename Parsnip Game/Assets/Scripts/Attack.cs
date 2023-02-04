using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputManager))]

public class Attack : MonoBehaviour
{
    private InputManager inputManager;
    public ParticleSystem scatterFx;

    private float attackProgress;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.attack.ReadValue<float>() == 1)
            DoAttack();
        else
            ReleaseAttack();
        
        Debug.Log(attackProgress);

        if (attackProgress >= 1)
            scatterFx.Emit(5000);
    }

    private void DoAttack()
    {
        if (attackProgress < 1) attackProgress += 0.5f * Time.deltaTime;

        if (attackProgress > 1) attackProgress = 1;
    }

    private void ReleaseAttack()
    {
        if (attackProgress > 0) attackProgress -= Time.deltaTime;

        if (attackProgress < 0) attackProgress = 0;
    }
}
