using UnityEditor;
using UnityEngine;

public class AdManagerAI

    : Singleton<AdManagerAI>
{
    public bool IsInGameView = false;

    private  bool _shouldShowBannerAd = false;

    public float adInterval = 3 * 60f; // Time interval to show ads in seconds
    
    public bool ShouldShowInterstitialAd = false;
    public float adTimer;
    public int matchCount = 0;
    public int matchesBeforeAd = 4; // Number of matches before showing an ad

    public bool ShouldShowBannerAd { get => _shouldShowBannerAd; private set =>  _shouldShowBannerAd = value; }


    public bool IsRewardedAdCanceled { get;private set; }


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // Initialize the ad timer
        adTimer = adInterval;
    }

    private void Update()
    {
        if (IsInGameView)
        {
            // If in-game, decrement the ad timer but do not reset it
            adTimer -= Time.deltaTime;

            if (adTimer <= 0)
            {
                // Set the flag to show ad after exiting game view
                ShouldShowInterstitialAd = true;
            }
        }
        else
        {
            // If not in-game, continue decrementing the ad timer
            adTimer -= Time.deltaTime;

            if (adTimer <= 0 && !ShouldShowInterstitialAd)
            {
                ShowInterstitialVideoAd();
                ResetAdTimerAndMatchCount();
            }

            // Check if the player has played enough matches within the ad interval
            if (matchCount >= matchesBeforeAd && !ShouldShowInterstitialAd)
            {
                ShowInterstitialVideoAd();
                ResetAdTimerAndMatchCount();
            }
        }
    }


    public void EnterGameView()
    {
        IsInGameView = true;
        matchCount++; // Increase the match count when entering game view
        // Additional logic when entering game view
        HideBannerAd();
        CancelShowingRewardedAd();
    }

    #region Banner Ad AI
    public void ShowBannerAd()
    {
        ShouldShowBannerAd = true;
        AdMobsAds.Instance.ShowBannerAd();
    }

    public void HideBannerAd()
    {
        ShouldShowBannerAd = false;
        AdMobsAds.Instance.HideBannerAd();
    }

    #endregion

    #region InterStial Ad AI

    public void ExitGameView()
    {
        IsInGameView = false;

        if (ShouldShowInterstitialAd)
        {
            ShowInterstitialVideoAd();
            ShouldShowInterstitialAd = false;
            ResetAdTimerAndMatchCount();
        }
    }

    private void ShowInterstitialVideoAd()
    {
        AdMobsAds.Instance.ShowOrLoadInterstitialAd();
        Debug.Log("Showing video ad...");
        // Replace with actual ad showing logic
        // AdProvider.ShowVideoAd();
    }

    private void ResetAdTimerAndMatchCount()
    {
        adTimer = adInterval; // Reset the ad timer
        matchCount = 0; // Reset the match count
    }

    #endregion

    #region Rewarded Ad AI
    public void LoadAndOrShowRewardedVideoAd()
    {
        IsRewardedAdCanceled = false;
        AdMobsAds.Instance.ShowOrLoadRewardedAd();
    }

    public void CancelShowingRewardedAd()
    {
        IsRewardedAdCanceled = true;
    }

    #endregion

}
