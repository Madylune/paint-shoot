using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float timeStart = 5;
    [SerializeField] private Text countdownText;

    private void Start()
    {
        countdownText.text = "( " + timeStart.ToString() + " )";
    }

    private void Update()
    {
        if (timeStart >= 0)
        {
            timeStart -= Time.deltaTime;
            countdownText.text = "( " + Mathf.Round(timeStart).ToString() + " )";
        }
        else
        {
            timeStart = 0;
            countdownText.text = "( 0 )";
            GameManager.MyInstance.QuitGame();
        }
    }
}
