using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool Playing { get; private set; } = false;
    public int KillCount { get; private set; }
    public float PlayTime { get; private set; }

    public static event Action OnGameSceneStarted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (Playing)
        {
            PlayTime += Time.deltaTime;
        }
    }

    public void AddKill()
    {
        KillCount++;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InitializeGameScene();
        }
    }

    private void InitializeGameScene()
    {
        Debug.Log("GameScene Intialiezed");

        Playing = true;
        KillCount = 0;
        PlayTime = 0;

        //오디오
        OnGameSceneStarted?.Invoke();

        //플레이어 초기화
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.InitializePlayer();
        }
        //좀비스포너 초기화
        var spawners = FindObjectsByType<ZombieSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            spawner.enabled = true;
            spawner.InitializeSpawner();
        }
    }

    public void OnPlayerDead()
    {
        GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Game over");

        Playing = false;

        GameOverUI.Instance.Show(KillCount, PlayTime);

        //SceneController.Instance.LoadMenu();
    }

    public void OnPlayerWin()
    {
        Debug.Log("Game Win!!!");

        Playing = false;

        GameOverUI.Instance.ShowWin(KillCount, PlayTime);

        Time.timeScale = 0f;
    }
}
