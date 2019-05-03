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
    [HideInInspector] public List<Transform> players = new List<Transform>();

    [Header("Game")]
    public GameStates gameState;
    public int levelIndex;
    public float maxTimer;
    [HideInInspector] public float timer;
    private bool rang;

    [Header("References")]
    public GameObject cameraPrefab;
    public VibrationManager vibro;
    internal Camera mainCam;
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
        timer -= Time.deltaTime;
        HUDManager.Instance.UpdateTimer(timer);

        if (!rang && maxTimer - timer <= 20.0f)
        {
            rang = true;
            SFXManager.Instance.EndTimerSFX();
        }

        if (timer <= 0)
        {
            gameState = GameStates.END_OF_LEVEL;
            SFXManager.Instance.WinJingleSFX();
            HUDManager.Instance.OnVictory();
        }
    }

    public void Lose()
    {
        gameState = GameStates.END_OF_LEVEL;
        SFXManager.Instance.LoseJingleSFX();
        HUDManager.Instance.OnDefeat();
    }

    public void NextScene()
    {
        levelIndex++;
        if (levelIndex < SceneManager.sceneCountInBuildSettings)
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
        players.Clear();
        timer = maxTimer;
        gameState = GameStates.PLAYING;
        Instantiate(hudManPrefab);
        GameObject cam = Instantiate(cameraPrefab, cameraPosition, Quaternion.Euler(cameraRotation));
        fovBooster = cam.GetComponentInChildren<FOVBooster>();
        shaker = cam.GetComponentInChildren<ScreenShaker>();
        mainCam = cam.GetComponentInChildren<Camera>();
        SFXManager.Instance.PlayThemeMusic();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnNewScene;
    }
}
