using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    public void ContinueHandler()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // чтобы работала после паузы 
    }
   
}
