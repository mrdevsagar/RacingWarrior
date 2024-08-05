using UnityEngine;
using TMPro;

public class DCTCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI diamondText;

    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] TextMeshProUGUI tokensText;

    void Start()
    {
        // Subscribe to the events

        GameManager.Instance.OnDiamondsChanged += UpdateDiamondDisplay;

        GameManager.Instance.OnCoinsChanged += UpdateCoinDisplay;

        TokenManager.Instance.OnTokensChanged += UpdateTokensDisplay;

        // Initial update

        UpdateDiamondDisplay(GameManager.Instance.GetDiamonds());
        UpdateCoinDisplay(GameManager.Instance.GetCoins());
        UpdateTokensDisplay(TokenManager.Instance.Tokens);
        
    }

    void Update()
    {
        
    }

    private void UpdateDiamondDisplay(int diamond)
    {
        if (diamondText != null)
        {
            diamondText.text = diamond.ToString(); // Update diamond display
        }
    }
    private void UpdateCoinDisplay(int coin)
    {
        if (coinText != null)
        {
            coinText.text = coin.ToString(); // Update coin display
        }
    }

    private void UpdateTokensDisplay(int tokens)
    {
        if (tokensText != null)
        {
            tokensText.text = tokens.ToString(); // Update tokens display
        }
    }

}
