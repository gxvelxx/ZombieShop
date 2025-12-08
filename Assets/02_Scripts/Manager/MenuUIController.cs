using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneController.Instance.LoadGame();
    }

    public void OnClickExit()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
