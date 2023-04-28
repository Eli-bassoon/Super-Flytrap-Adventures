using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    int deaths;
    [SerializeField] TextMeshProUGUI deathCounter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deaths = DamageHandler.instance.deaths;
        deathCounter.SetText(deaths.ToString() + "\nDEATHS");
    }
}
