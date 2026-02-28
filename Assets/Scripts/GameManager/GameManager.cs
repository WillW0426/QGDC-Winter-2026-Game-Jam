using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused
    }

    public GameState currentState;

    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseMenu;

    private void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            currentState = GameState.Playing;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    public GameState GetGameState()
    {
        return currentState;
    }
}
