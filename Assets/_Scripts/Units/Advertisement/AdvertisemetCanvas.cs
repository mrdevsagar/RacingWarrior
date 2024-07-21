using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisemetCanvas : MonoBehaviour
{
    [SerializeField] Button _loadBannerButton;
    [SerializeField] Button _hideButton;
    [SerializeField] Button _showButton;


    [SerializeField] Button _laodIntertialButton;
    [SerializeField] Button _showntertialButton;

    [SerializeField] Button _shownRewardedAdButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _loadBannerButton.onClick.AddListener(LoadBanner);
        _hideButton.onClick.AddListener(hideUnhideBanner);
        _showButton.onClick.AddListener(ShowBanner);

        _laodIntertialButton.onClick.AddListener(LoadIntertialAd);
        _showntertialButton.onClick.AddListener(ShowIntertialAd);
        _shownRewardedAdButton.onClick.AddListener(ShowRewardedAd);
    }

    // Update is called once per frame
    void Update()
    {
        
            _shownRewardedAdButton.gameObject.SetActive(!AdMobsAds.Instance.IsLoadingRewardedAd);
        
    }

    private void hideUnhideBanner()
    {
        AdMobsAds.Instance.HideBannerAd();
    }

    private void ShowBanner()
    {
        AdMobsAds.Instance.ShowBannerAd();
    }



    private void LoadBanner()
    {
       
        AdMobsAds.Instance.LoadBannerAd();


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
        AdMobsAds.Instance.LoadRewardedAd();
    }


    
}

