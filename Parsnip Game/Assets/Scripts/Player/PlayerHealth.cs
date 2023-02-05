using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public int healthPoints = 3;

    void GameOver()
    {
        if(healthPoints <= 0) { SceneManager.LoadScene("GameOver"); }
    }

    private void OnEnable()
    {
        FarmerBehaviour.loseHealth += GameOver;
    }

    private void OnDisable()
    {
        FarmerBehaviour.loseHealth -= GameOver;    
    }
}
