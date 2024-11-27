using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject EndScenes;
    private int endTime = 300;
    private float currentTime = 0;
    private bool isTimerRunning = false;
    void Start()
    {
        currentTime = endTime;
        isTimerRunning = true;
        UpdateTimerText();       
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; 
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isTimerRunning = false;
                OnTimerEnd(); 
            }
            UpdateTimerText(); 
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("StartSence");
            }
        }
    }
    
    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(currentTime).ToString(); 
        }
    }

    void OnTimerEnd()
    {
        Time.timeScale = 0f; 
        EndScenes.SetActive(true);
    }
}
