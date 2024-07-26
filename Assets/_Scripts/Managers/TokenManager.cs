using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class TokenManager : Singleton<TokenManager>
{
    public bool IsLogging = false;
 /*   // Singleton instance
    private static TokenManager _instance;*/

    private const float ripitTokenIncrimentRate = 3f;

    public int Tokens { get => _tokens;private set => _tokens = value; }

    private const string TOKEN_KEY = "Tokens";
    private const string LAST_UPDATE_TIME_KEY = "LastUpdateTime";

    private int _tokens = 0;
    private readonly int _tokenIncrementRate = 1; // Tokens per second
    private readonly float _tokenIncrementInterval = 1.0f; // Interval in seconds for token incrementation

    // Event to notify UI about token count changes
    public UnityAction<int> OnTokensChanged;

    protected override void Awake()
    {
        base.Awake();
        // Your initialization code here

    }

    private void Start()
    {
        LoadTokens();
        CancelInvoke(nameof(IncrementTokens));
        InvokeRepeating(nameof(IncrementTokens), ripitTokenIncrimentRate, ripitTokenIncrimentRate); // Start incrementing tokens every second
    }


    private void IncrementTokens()
    {
        Log("Incrementing Token Count", "TokenManager");
        if (Tokens < 1000)
        {

            Tokens += _tokenIncrementRate;

            // Notify listeners that tokens have changed
            OnTokensChanged?.Invoke(Tokens);

            SaveTokens();

            /*// Update last update time
            SaveLastUpdateTime();*/
        } else
        {
            CancelInvoke(nameof(IncrementTokens));
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
        if(Tokens < 1000) {
            Tokens += increments * _tokenIncrementRate;
            if (Tokens > 1000)
            {
                Tokens = 1000;
            }
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

    public void AddToken(int  tokens)
    {
        Tokens += tokens;
        OnTokensChanged?.Invoke(Tokens);
    }

    // Function to spend tokens
    public bool SpendTokens(int amount)
    {
        if (Tokens >= amount)
        {
            Tokens -= amount;
            SaveTokens();
            OnTokensChanged?.Invoke(Tokens);
            CancelInvoke(nameof(IncrementTokens));
            InvokeRepeating(nameof(IncrementTokens), ripitTokenIncrimentRate, ripitTokenIncrimentRate);
            return true; // Tokens successfully spent
        }
        else
        {
            return false; // Insufficient tokens to spend
        }
    }

    public void Log(object message,string callsName)
    {
        if (IsLogging)
        {
            Debug.Log(message + " : " + callsName);
        }
        
    }
}
