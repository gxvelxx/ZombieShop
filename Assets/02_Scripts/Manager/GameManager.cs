using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool Playing { get; private set; } = false;

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
        }
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

        SceneController.Instance.LoadMenu();
    }
}
