using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

public class DCTCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _diamondText;

    [SerializeField] TextMeshProUGUI _coinText;

    [SerializeField] TextMeshProUGUI _tokensText;

    [SerializeField] List<GameObject> _addIcons;

    [SerializeField] GameObject _panelDCT;

    [SerializeField] private CanvasGroup _panelDCTCanvasGroup;

    public ScrollRect scrollRect; // Assign the ScrollRect component
    public float targetHorizontalPosition = 1.0f; // Target vertical position (0 = bottom, 1 = top)
    public float scrollDuration = 0.5f; // Duration of the scroll animation

    private bool isScrolling = false;

    public float aaa = 0f;

    // Call this method on button click
   
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
        ScrollToPosition(0f);
    }

    public void AddCoin()
    {
        ShowDCTCanvas();
        ScrollToPosition(0.3f);
    }

    public void AddToken()
    {
        ShowDCTCanvas();
        ScrollToPosition(1f);

    }

    private void ShowDCTCanvas()
    {
        _panelDCTCanvasGroup.alpha = 1;
        _panelDCTCanvasGroup.blocksRaycasts = true;
        DisableAddIcon();
    }

    public void CloseDCTCanvas()
    {
        _panelDCTCanvasGroup.alpha = 0;
        _panelDCTCanvasGroup.blocksRaycasts = false;
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

    public void ScrollToPosition(float position)
    {
        targetHorizontalPosition = position;
        if (!isScrolling)
        {
            StartCoroutine(ScrollCoroutine(targetHorizontalPosition));
        }
    }

    private IEnumerator ScrollCoroutine(float targetPosition)
    {
        isScrolling = true;
        float startVerticalPosition = scrollRect.horizontalNormalizedPosition;
        float elapsedTime = 0f;

        while (elapsedTime < scrollDuration)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startVerticalPosition, targetPosition, elapsedTime / scrollDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
        isScrolling = false;
    }

}
