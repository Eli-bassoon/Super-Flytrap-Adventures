using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] GameObject audioSettings;
    [SerializeField] GameObject gameplaySettings;
    [SerializeField] Button audioTabButton;
    [SerializeField] Button gameplayTabButton;
    [SerializeField] Color tabColor;

    Color inactiveColor = new Color(0.6f, 0.6f, 0.6f);

    TextMeshProUGUI audioButtonText;
    TextMeshProUGUI gameplayButtonText;

    private bool viewingAudio;
    // to do: disable audio/gameplay depending on which button was last pressed

    // Start is called before the first frame update
    void Start()
    {
        viewingAudio = true;
        audioSettings.SetActive(true);
        gameplaySettings.SetActive(false);

        audioButtonText = audioTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameplayButtonText = gameplayTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (viewingAudio)
        {
            audioButtonText.color = tabColor;
            gameplayButtonText.color = inactiveColor; 
        }
        else
        {
            audioButtonText.color = inactiveColor;
            gameplayButtonText.color = tabColor;
        }
    }

    public void onClickAudioTab()
    {
        if (viewingAudio) return;
        toggleTabs();
    }

    public void onClickGameplayTab()
    {
        if (!viewingAudio) return;
        toggleTabs();
    }

    void toggleTabs()
    {
        viewingAudio = !audioSettings.activeSelf;
        audioSettings.SetActive(viewingAudio);
        gameplaySettings.SetActive(!viewingAudio);
        
        //audioTabButton.transform.GetChild(0).GetComponent<TextMeshPro>
    }
}
