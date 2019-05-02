using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { PLAYING, END_OF_LEVEL }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    [Header("Game")]
    public GameStates gameState;
    public int levelIndex;
    public float levelTimer;
    private float time;

    [Header("References")]
    public GameObject cameraPrefab;
    public VibrationManager vibro;
    internal ScreenShaker shaker;
    internal FOVBooster fovBooster;
    public GameObject hudManPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnNewScene;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameStates.PLAYING:
                UpdateTimer();
                break;
        }
    }

    private void UpdateTimer()
    {
        time -= Time.deltaTime;
        HUDManager.Instance.UpdateTimer(time);

        if (time <= 0)
        {
            gameState = GameStates.END_OF_LEVEL;
            HUDManager.Instance.OnVictory();
        }
    }

    public void Lose()
    {
        gameState = GameStates.END_OF_LEVEL;
        HUDManager.Instance.OnDefeat();
    }

    public void NextScene()
    {
        levelIndex++;
        if (levelIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(levelIndex);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(levelIndex);
    }

    private void OnNewScene(Scene scene, LoadSceneMode mode)
    {
        time = levelTimer;
        gameState = GameStates.PLAYING;
        Instantiate(hudManPrefab);
        GameObject cam = Instantiate(cameraPrefab, cameraPosition, Quaternion.Euler(cameraRotation));
        fovBooster = cam.GetComponentInChildren<FOVBooster>();
        shaker = cam.GetComponentInChildren<ScreenShaker>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnNewScene;
    }
}
