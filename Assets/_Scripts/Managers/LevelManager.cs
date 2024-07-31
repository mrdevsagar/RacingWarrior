using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : SingletonLocal<LevelManager>
{
    /// <summary>
    /// Disable this thing in production
    /// </summary>
    [SerializeField]
    private bool _isAutoPauseEnabled = false;

    [SerializeField]
    private bool _isGameEnded = false;


    public bool IsPlayerWon { get ; set; }
    public bool IsGamePaused { get; set; }
    public bool IsGameEnded { get => _isGameEnded; set => _isGameEnded = value; }

    public GameObject pauseScreen; // Reference to the pause screen UI

    public GameObject resultScreen;

    [SerializeField]
    private Button button;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    // This method will be called when the button is clicked
    private void OnButtonClick()
    {
        if(!IsGameEnded)
        {
            ShowPauseScreen();
        } else
        {
            ShowResultScreen();
        }
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
        // UnSubscribe from the focusChanged event
        Application.focusChanged -= OnFocusChanged;
    }

    private void OnFocusChanged(bool hasFocus)
    {
        if(!_isAutoPauseEnabled)
        {
            return;
        }

        if (!hasFocus)
        {
            if(IsGameEnded)
            {
                ShowResultScreen();
            } else
            {
                ShowPauseScreen();
            }
        }
        else
        {
            if(IsGameEnded)
            {
                ShowResultScreen();
            }
        }
    }

    public void ResumeGame()
    {
        if(!IsGameEnded)
        {
            HidePauseScreen();
            GameManager.Instance.PauseGame(false);
        }
    }
    public void ToggleIsGameEnded()
    {
        IsGameEnded = !IsGameEnded;
    }

    private void ShowPauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
            GameManager.Instance.PauseGame(true);
        }
    }

    private void HidePauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }

    private void ShowResultScreen()
    {
        GameManager.Instance.PauseGame(true);
        if (IsGameEnded)
        {
            HidePauseScreen();
            if (resultScreen != null)
            {
                resultScreen.SetActive(true);
            }
        }
    }
    
}
