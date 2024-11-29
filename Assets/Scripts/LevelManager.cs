using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float CustomerGeneratingTime;
    public float CustomerWaitingTime;

    public int Level = -1;
    public TMP_Text LevelShow;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Level!= (int)OrderingManager.Instance.AllMoney / 100) // level up for every 100 money
        {
            Level = (int)OrderingManager.Instance.AllMoney / 100;
            LevelShow.text = $"{Level + 1}";
            CustomerGeneratingTime = 15 - Level; 
            // generate customer for 15 sec from start; generating time decreases for each second

            int tmpint = 50 - Level * 5; // Set a minimum waiting time
            int tmpint2 = 20 - (Level - 6) * 2;  // waitingTime < 20, customer waiting time decreases for 2 sec for each level
            CustomerWaitingTime = tmpint>20?tmpint: tmpint2;
        }
    }
}
