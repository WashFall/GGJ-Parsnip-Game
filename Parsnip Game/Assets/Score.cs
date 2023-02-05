using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    public TMP_Text text;
    public int scoreing;
    public int winScore;
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void AddScore(int score)
    {
        scoreing += score;
        text.text = $"Score: {scoreing}";
        
        if(scoreing >= winScore)
        {
            SceneManager.LoadScene(3);
        }
    }
}
