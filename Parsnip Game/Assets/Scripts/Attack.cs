using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private CharacterControls characterControls;

    private void Start()
    {
        characterControls = new CharacterControls();
            
    }
}
