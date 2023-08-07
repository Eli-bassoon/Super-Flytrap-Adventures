using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    int deaths;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        deaths = DamageHandler.instance.deaths;
        string plural = deaths == 1 ? "" : "S";
        text.SetText($"{deaths}\nDEATH{plural}");
    }
}
