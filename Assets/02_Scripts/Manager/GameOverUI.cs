using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    public GameObject _panel;
    public Text _killText;
    public Text _timeText;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    public void Show(int kills, float playTime)
    {
        _panel.SetActive(true);

        _killText.text = $"Kills : {kills}";
        _timeText.text = $"Time : {playTime:F1}sec";

        Time.timeScale = 0f; // 게임 정지        
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadGame();
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadMenu();
    }
}
