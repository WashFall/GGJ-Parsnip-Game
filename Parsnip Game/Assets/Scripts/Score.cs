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

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GetText();
    }

    public void AddScore(int score)
    {
        scoreing += score;
        text.text = $"Score: {scoreing}";
        
        if(scoreing >= winScore)
        {
            SceneManager.LoadScene(2);
        }
    }

    private void GetText()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "MainMenu") return;

        text = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();

        if (scene.name == "Main") return;

        SetScoreTextOnEndScenes();

    }

    public void SetText()
    {
        text.text = $"Score: {scoreing}";
    }

    private void SetScoreTextOnEndScenes()
    {
        text.text = $"Score: {scoreing}";
    }
}
