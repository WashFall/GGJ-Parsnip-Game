using UnityEngine;
using TMPro;

public class TimeLimit : MonoBehaviour
{
    public float timeSpanInMinutes;

    private float timeLeft;
    private float startTime;
    private TMP_Text timerText;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        startTime = Time.time;
        timeSpanInMinutes *= 60;
    }

    void Update()
    {
        float timerCount = Time.time - startTime;
        timeLeft = timeSpanInMinutes - timerCount;
        float minutes = Mathf.Floor(timeLeft / 60);
        float seconds = Mathf.Floor(timeLeft % 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}