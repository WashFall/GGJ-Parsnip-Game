using UnityEngine;
using TMPro;

public class TimeLimit : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeSpan;

    private float timeLeft;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        timeSpan *= 60;
    }

    void Update()
    {
        float timerCount = Time.time - startTime;
        timeLeft = timeSpan - timerCount;
        float minutes = Mathf.Floor(timeLeft / 60);
        float seconds = Mathf.Floor(timeLeft % 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}