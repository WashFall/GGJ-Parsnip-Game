using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class SetText : MonoBehaviour
{
    void Start()
    {
        Score.Instance.text = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();
        Score.Instance.SetText();
    }

}
