using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    // Game variables
    private int coins = 0;
    private int diamonds = 0;
    

    // Events for UI or other systems to subscribe to changes
    public UnityAction<int> OnCoinsChanged;
    public UnityAction<int> OnDiamondsChanged;

    protected override void Awake()
    {
        base.Awake();
        // Your initialization code here

        LoadGameData();
    }

    #region CoinAndDiamonds
    // Getters for coins and diamonds
    public int GetCoins()
    {
        return coins;
    }

    public int GetDiamonds()
    {
        return diamonds;
    }

    // Transaction functions
    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
        SaveGameData();
    }

    public void AddDiamonds(int amount)
    {
        diamonds += amount;
        OnDiamondsChanged?.Invoke(diamonds);
        SaveGameData();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke(coins);
            SaveGameData();
            return true; // Transaction successful
        }
        else
        {
            return false; // Insufficient coins
        }
    }

    public bool SpendDiamonds(int amount)
    {
        if (diamonds >= amount)
        {
            diamonds -= amount;
            OnDiamondsChanged?.Invoke(diamonds);
            SaveGameData();
            return true; // Transaction successful
        }
        else
        {
            return false; // Insufficient diamonds
        }
    }

    // Method to save game data
    private void SaveGameData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Diamonds", diamonds);
        PlayerPrefs.Save();
    }

    // Method to load game data
    private void LoadGameData()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        diamonds = PlayerPrefs.GetInt("Diamonds", 0);
    }
    #endregion


    public void SwitchScene(string sceneName,bool isGameView)
    {
        SceneManager.LoadSceneAsync(sceneName);
        /*if(isGameView)
        {
            AdManagerAI.Instance.EnterGameView();
        } else
        {
            AdManagerAI.Instance.ExitGameView();
        }*/
    }
}
