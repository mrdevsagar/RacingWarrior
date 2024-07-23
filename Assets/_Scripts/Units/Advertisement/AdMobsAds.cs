//TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713"

//REAL_APP_ID
//"ca-app-pub-7191923771378224~6783351723"

//! BEFOR CHANGING TO REAL ID'S FOR ANDROID SET THE "GOOGLE MOBILE ADS APP ID" TO ABOVE REAL_APP_ID ()
// LOCATION (Assets/ Google Mobile Ads/ Settings)



#define IS_USE_REAL_AD_IDS


using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System.Collections;

/// <summary>
///   AdMobsAds for managing AdMob ads with singleton instance.
/// </summary>
public class AdMobsAds : Singleton<AdMobsAds>
{

#if UNITY_ANDROID

    #if IS_USE_REAL_AD_IDS
        //Real ID's 
        readonly string BANNER_ID = "ca-app-pub-7191923771378224/9446040059";
        readonly string INTERSTIAL_ID = "ca-app-pub-7191923771378224/4353463651";
        readonly string REWARDED_ID = "ca-app-pub-7191923771378224/1567550036";
        readonly string NATIVE_ID = "ca-app-pub-7191923771378224/1727300312";
    #else
        //Test ID's
        readonly  string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        readonly  string INTERSTIAL_ID = "ca-app-pub-3940256099942544/1033173712";
        readonly string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
        readonly   string NATIVE_ID = "ca-app-pub-3940256099942544/2247696110";
    #endif

#elif UNITY_IPHONE

    string BANNER_ID = "ca-app-pub-3940256099942544/2934735716";
    string INTERSTIAL_ID = "ca-app-pub-3940256099942544/4411468910";
    string REWARDED_ID = "ca-app-pub-3940256099942544/1712485313";
    string NATIVE_ID = "ca-app-pub-3940256099942544/3986624511";

#endif

    BannerView _bannerView;
    InterstitialAd _interstitialAd;
    RewardedAd _rewardedAd;
    NativeAd _nativeAd;

   
    private GameObject _videoErrorCanvasPrefab;
    private GameObject _videoErrorCanvas;

    public GameObject VideoErrorCanvas { get => _videoErrorCanvas;private set => _videoErrorCanvas = value; }

    
    public bool IsLoadingInterstitialAd { get; private set; }

    public bool IsLoadingRewardedAd { get; private set; }

    #region SingletonInstance Code

   

    protected override void Awake()
    {
        base.Awake();
        _videoErrorCanvasPrefab = Resources.Load<GameObject>("RewaredAddErrorCancas");
        VideoErrorCanvas = Instantiate(_videoErrorCanvasPrefab);
        // Set the canvas as a child of the GameManager GameObject
        VideoErrorCanvas.transform.SetParent(this.transform);
    }


    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialized !!");

        });
    }
    #endregion

    #region Banner Ads

    /// <summary>
    /// Loads a new banner ad every time.
    /// </summary>
    public void LoadBannerAd()
    {
        //create a banner
        CreateBannerView();

        //listen to banner events
        ListenToBannerEvents();

        //load the banner
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        _bannerView.LoadAd(adRequest);//show the banner on the screen

        
    }
    void CreateBannerView()
    {
        if (_bannerView != null)
        {
            DestroyBannerAd();
        }
        _bannerView = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Top);
    }
    void ListenToBannerEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            print("Destroying banner Ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    /// <summary>
    /// Hides the Existing banner add.
    /// </summary>
    public void HideBannerAd()
    {
        _bannerView?.Hide();
    }

    /// <summary>
    /// Shows the Existing banner add.
    /// </summary>
    public void ShowBannerAd()
    {
        _bannerView?.Show();
    }

    #endregion

    #region Interstitial

    public void LoadInterstitialAd()
    {
        IsLoadingInterstitialAd = true;
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(INTERSTIAL_ID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                IsLoadingInterstitialAd = false;
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            _interstitialAd = ad;
            InterstitialEvent(_interstitialAd);
            ShowInterstitialAd();
        });

    }
    public void ShowInterstitialAd()
    {

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
            IsLoadingInterstitialAd = false;
        }
        else
        {
            print("Interstitial ad not ready!!");
        }
    }
    private void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {
        IsLoadingRewardedAd = true;
        _videoErrorCanvas.SetActive(false);
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(REWARDED_ID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                _videoErrorCanvas.SetActive(true);
                print("Rewarded failed to load" + error);
                return;
            }

            print("Rewarded ad loaded !!");
            _rewardedAd = ad;
            RewardedAdEvents(_rewardedAd);
            ShowRewardedAd();
        });
    }
    public void ShowRewardedAd()
    {
        IsLoadingRewardedAd = false;
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player !!");


            });
        }
        else
        {
            print("Rewarded ad not ready");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion


    #region Native

    public Image img;

    public void RequestNativeAd()
    {

        AdLoader adLoader = new AdLoader.Builder(NATIVE_ID).ForNativeAd().Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        adLoader.LoadAd(new AdRequest());
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        print("Native ad loaded");
        this._nativeAd = e.nativeAd;

        Texture2D iconTexture = this._nativeAd.GetIconTexture();
        Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * .5f);

        img.sprite = sprite;

    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        print("Native ad failed to load" + e.ToString());

    }
    #endregion


    public void PausseGame()
    {

    }

    public void PlayGame()
    {

    }

    public IEnumerable LoadAds()
    {
        return null;
    }

    public void CancelAllVideoAdsRequest()
    {
        _rewardedAd = null;
        _interstitialAd = null;
    }

    
}