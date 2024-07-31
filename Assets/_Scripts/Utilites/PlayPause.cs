using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayPause : MonoBehaviour
{
    public GameObject pauseScreen; // Reference to the pause screen UI


    private Button button;

    private bool isPaused = false;

    void Awake()
    {
        // Get the Button component attached to the same GameObject
        button = GetComponent<Button>();

        // Add an OnClick listener
        button.onClick.AddListener(OnButtonClick);
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        TogglePause();
    }

    void OnDestroy()
    {
        // Remove the listener when the object is destroyed to prevent memory leaks
        button.onClick.RemoveListener(OnButtonClick);
    }

    void OnEnable()
    {
        // Subscribe to the focusChanged event
        Application.focusChanged += OnFocusChanged;
    }

    void OnDisable()
    {
        // Unsubscribe from the focusChanged event
        Application.focusChanged -= OnFocusChanged;
    }

    private void OnFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            ShowPauseScreen();
        }
        else
        {
            HidePauseScreen();
        }
    }

    private void ShowPauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0; // Pause the game
            Debug.Log("Game paused and pause screen shown");
        }
    }

    private void HidePauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1; // Resume the game
            Debug.Log("Game resumed and pause screen hidden");
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }
}
