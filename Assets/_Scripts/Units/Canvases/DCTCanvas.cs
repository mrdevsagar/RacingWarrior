using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;

public class DCTCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _diamondText;

    [SerializeField] TextMeshProUGUI _coinText;

    [SerializeField] TextMeshProUGUI _tokensText;

    [SerializeField] List<GameObject> _addIcons;

    [SerializeField] GameObject _panelDCT;

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
        if (_diamondText != null)
        {
            _diamondText.text = diamond.ToString(); // Update diamond display
        }
    }
    private void UpdateCoinDisplay(int coin)
    {
        if (_coinText != null)
        {
            _coinText.text = coin.ToString(); // Update coin display
        }
    }

    private void UpdateTokensDisplay(int tokens)
    {
        if (_tokensText != null)
        {
            _tokensText.text = tokens.ToString(); // Update tokens display
        }
    }

    public void AddDiamond()
    {
        ShowDCTCanvas();
    }

    public void AddCoin()
    {
        ShowDCTCanvas();
    }

    public void AddToken()
    {
        ShowDCTCanvas();
    }

    private void ShowDCTCanvas()
    {
        _panelDCT.SetActive(true);
        DisableAddIcon();
    }

    public void CloseDCTCanvas()
    {
        _panelDCT.SetActive(false);
        EnableAddIcon();
    }

    private void DisableAddIcon()
    {
        foreach (var obj in _addIcons)
        {
            obj.SetActive(false);
        }
    }

    private void EnableAddIcon()
    {
        foreach (var obj in _addIcons)
        {
            obj.SetActive(true);
        }
    }



}
