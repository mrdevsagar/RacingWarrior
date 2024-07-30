using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisemetCanvas : MonoBehaviour
{
    [SerializeField] string switchScreenName;
    [SerializeField] bool isGameView;

    [SerializeField] Button _loadBannerButton;
    [SerializeField] Button _hideButton;
    [SerializeField] Button _showButton;


    [SerializeField] Button _laodIntertialButton;
    [SerializeField] Button _showntertialButton;

    [SerializeField] Button _shownRewardedAdButton;

    [SerializeField] Button _spendTokenButton;

    [SerializeField] Button _addCoin;
    [SerializeField] Button _spendCoin;

    [SerializeField] Button _addDiamond;
    [SerializeField] Button _spendDiamond;

    [SerializeField] Button _addTokens;

    [SerializeField] Button _switchScreenButton;

    [SerializeField] TextMeshProUGUI  tokensText;

    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] TextMeshProUGUI diamondText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _loadBannerButton.onClick.AddListener(LoadBanner);
        _hideButton.onClick.AddListener(hideUnhideBanner);
        _showButton.onClick.AddListener(ShowBanner);

        _laodIntertialButton.onClick.AddListener(LoadIntertialAd);
        _showntertialButton.onClick.AddListener(ShowIntertialAd);
        _shownRewardedAdButton.onClick.AddListener(ShowRewardedAd);
        _spendTokenButton.onClick.AddListener(SpendToken);

        _addCoin.onClick.AddListener(AddCoins);
        _spendCoin.onClick.AddListener(SpendCoins);

        _addDiamond.onClick.AddListener(AddDiamonds);
        _spendDiamond.onClick.AddListener(SpenDiamonds);

        _addTokens.onClick.AddListener(AddTokens);

        _switchScreenButton.onClick.AddListener(SwitchScene);


        // Subscribe to the OnTokensChanged event
        TokenManager.Instance.OnTokensChanged += UpdateTokensDisplay;

        GameManager.Instance.OnCoinsChanged += UpdateCoinDisplay;

        GameManager.Instance.OnDiamondsChanged += UpdateDiamondDisplay;

        // Initial update of tokens display
        UpdateTokensDisplay(TokenManager.Instance.Tokens);
        UpdateCoinDisplay(GameManager.Instance.GetCoins());
        UpdateDiamondDisplay(GameManager.Instance.GetDiamonds());
    }

    private void OnDestroy()
    {
        /*// UnSubscribe from the OnTokensChanged event
        TokenManager.Instance.OnTokensChanged -= UpdateTokensDisplay;*/
    }

    // Update is called once per frame
    void Update()
    {
        
        _shownRewardedAdButton.gameObject.SetActive(!AdMobsAds.Instance.IsLoadingRewardedAd);

     /*   _switchScreenButton.gameObject.SetActive(!AdMobsAds.Instance.IsLoadingInterstitialAd);*/

    }

    private void UpdateTokensDisplay(int tokens)
    {
        if (tokensText != null)
        {
            tokensText.text = $"Tokens: {tokens}"; // Update tokens display
        }
    }

    private void UpdateCoinDisplay(int coin)
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coin}"; // Update coin display
        }
    }

    private void UpdateDiamondDisplay(int diamond)
    {
        if (diamondText != null)
        {
            diamondText.text = $"Diamonds: {diamond}"; // Update diamond display
        }
    }

    private void hideUnhideBanner()
    {
        AdManagerAI.Instance.HideBannerAd();
    }

    private void ShowBanner()
    {
        AdManagerAI.Instance.ShowBannerAd();
    }

   

    private void LoadBanner()
    {

        AdManagerAI.Instance.ShowBannerAd();


    }

    private void LoadIntertialAd()
    {
        AdMobsAds.Instance.LoadInterstitialAd();
    }

    private void ShowIntertialAd()
    {
        AdMobsAds.Instance.ShowInterstitialAd();
    }

    private void ShowRewardedAd()
    {
        AdManagerAI.Instance.LoadAndOrShowRewardedVideoAd("Congratulation","You have received",10,Collectible.COIN);
    }

    private void AddTokens()
    {
        TokenManager.Instance.AddToken(100);
    }
    private void SpendToken()
    {
        TokenManager.Instance.SpendTokens(50);
    }

    private void AddCoins()
    {
        GameManager.Instance.AddCoins(100);
    }

    private void SpendCoins()
    {
        GameManager.Instance.SpendCoins(20);
    }

    private void AddDiamonds()
    {
        GameManager.Instance.AddDiamonds(100);
    }

    private void SpenDiamonds()
    {
        GameManager.Instance.SpendDiamonds(20);
    }

    public  void SwitchScene()
    {
        AdMobsAds.Instance.SwitchSceneByShowingAd(switchScreenName);
    }

}

