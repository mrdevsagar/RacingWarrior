using System;
using UnityEngine;
using UnityEngine.Events;

public class TokenManager : MonoBehaviour
{
    // Singleton instance
    private static TokenManager _instance;

    // Public accessor for singleton instance
    public static TokenManager Instance
    {
        get
        {
            // If instance doesn't exist, find it in the scene or create a new one
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<TokenManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("TokenManager");
                    _instance = singletonObject.AddComponent<TokenManager>();
                }
            }
            return _instance;
        }
    }

    public int Tokens { get => _tokens;private set => _tokens = value; }

    private const string TOKEN_KEY = "Tokens";
    private const string LAST_UPDATE_TIME_KEY = "LastUpdateTime";

    private int _tokens = 0;
    private int _tokenIncrementRate = 1; // Tokens per second
    private float _tokenIncrementInterval = 1.0f; // Interval in seconds for token incrementation

    // Event to notify UI about token count changes
    public UnityAction<int> OnTokensChanged;

    private void Awake()
    {
        // Ensure only one instance of the singleton exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the GameObject when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        LoadTokens();
        InvokeRepeating("IncrementTokens", 1.0f, 1.0f); // Start incrementing tokens every second
    }

    public void Instantiate()
    {

    }

    private void IncrementTokens()
    {
        if (Tokens < 1000)
        {

            Tokens += _tokenIncrementRate;

            // Notify listeners that tokens have changed
            OnTokensChanged?.Invoke(Tokens);

            SaveTokens();

            /*// Update last update time
            SaveLastUpdateTime();*/
        }
    }

    private void LoadTokens()
    {
        // Calculate elapsed time since last token update
        DateTime lastUpdateTime = DateTime.Parse(PlayerPrefs.GetString(LAST_UPDATE_TIME_KEY, DateTime.UtcNow.ToString()));
        TimeSpan elapsedTime = DateTime.UtcNow - lastUpdateTime;

        // Calculate number of increments since last update
        int increments = (int)(elapsedTime.TotalSeconds / _tokenIncrementInterval);
        Tokens = PlayerPrefs.GetInt(TOKEN_KEY, 0);
        // Increment tokens based on elapsed time
        Tokens += increments * _tokenIncrementRate;
        if (Tokens > 1000)
        {
            Tokens = 1000;
        }
       
        
        OnTokensChanged?.Invoke(Tokens);
    }

    private void SaveTokens()
    {
        PlayerPrefs.SetInt(TOKEN_KEY, Tokens);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveTokens(); // Save tokens when the application is closed
        
        SaveLastUpdateTime();// Update last update time
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTokens(); // Save tokens when the application is paused (e.g., minimized)
            SaveLastUpdateTime();// Update last update time
        }
    }

    private void SaveLastUpdateTime()
    {
        PlayerPrefs.SetString(LAST_UPDATE_TIME_KEY, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    // Function to spend tokens
    public bool SpendTokens(int amount)
    {
        if (Tokens >= amount)
        {
            Tokens -= amount;
            SaveTokens();
            OnTokensChanged?.Invoke(Tokens);
            return true; // Tokens successfully spent
        }
        else
        {
            return false; // Insufficient tokens to spend
        }
    }
}
