using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    public GameObject victory;
    public GameObject defeat;
    public Button next;
    public Button retry;
    public GameObject exitButton;
    public Text timer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void UpdateTimer(float _timer)
    {
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timer.text = niceTime;
    }

    public void OnVictory()
    {
        timer.gameObject.SetActive(false);
        victory.SetActive(true);
        //EventSystem.current.firstSelectedGameObject = next;
        next.Select();
        next.OnSelect(null);
        exitButton.SetActive(true);
    }

    public void OnDefeat()
    {
        timer.gameObject.SetActive(false);
        defeat.SetActive(true);
        //EventSystem.current.firstSelectedGameObject = retry;
        exitButton.SetActive(true);
    }

    public void OnPressedNext()
    {
        GameManager.Instance.NextScene();
    }

    public void OnPressedRetry()
    {
        GameManager.Instance.ReloadScene();
    }

    public void OnPressedExit()
    {
        Application.Quit();
    }
}
