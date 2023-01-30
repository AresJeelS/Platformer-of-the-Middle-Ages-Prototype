using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteCanvas;
    [SerializeField] private GameObject messageUi;

    private bool _isActivated;

    public void Activate()
    {
        _isActivated = true;
        messageUi.SetActive(false);
    }
    public void FinishLevel()
    {
        if (_isActivated)
        {
            levelCompleteCanvas.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            messageUi.SetActive(true);
        }
    }
}
