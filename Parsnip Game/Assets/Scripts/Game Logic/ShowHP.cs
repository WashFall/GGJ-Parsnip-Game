using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowHP : MonoBehaviour
{
    public PlayerHealth hp;

    TMP_Text hpText;

    private void Start()
    {
        hpText = GetComponent<TMP_Text>();
    }

    void UpdateUI()
    {
        hpText.text = "HP: " + hp.healthPoints;
    }

    private void OnEnable()
    {
        FarmerBehaviour.loseHealth += UpdateUI;
    }

    private void OnDisable()
    {
        FarmerBehaviour.loseHealth -= UpdateUI;
    }
}
