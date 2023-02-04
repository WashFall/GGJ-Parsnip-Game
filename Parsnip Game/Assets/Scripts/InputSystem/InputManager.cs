using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private CharacterControls characterControls;
    public InputAction vertical, horizontal, attack;

    private void Awake()
    {
        characterControls = new CharacterControls();
        characterControls.Enable();
        vertical = characterControls.Movement.Vertical;
        horizontal = characterControls.Movement.Horizontal;
        attack = characterControls.Attack.Attack;
    }
}
