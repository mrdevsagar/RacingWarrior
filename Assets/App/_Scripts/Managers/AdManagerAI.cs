
using UnityEngine;

public class AdManagerAI

    : Singleton<AdManagerAI>
{

    private bool _shouldShowBannerAd = false;

    public bool ShouldShowBannerAd { get => _shouldShowBannerAd; private set => _shouldShowBannerAd = value; }

    public bool IsRewardedAdCanceled { get; private set; }


    protected override void Awake()
    {
        base.Awake();
    }

    public void EnterGameView()
    {
        HideBannerAd();
        CancelShowingRewardedAd();
    }
    public void ExitGameView()
    {

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

    #region Rewarded Ad AI
    public void LoadAndOrShowRewardedVideoAd(string title,string subTitle,int count, Collectible collectibleType)
    {
        IsRewardedAdCanceled = false;
        AdMobsAds.Instance.ShowOrLoadRewardedAd(title, subTitle, count, collectibleType);
    }

    public void CancelShowingRewardedAd()
    {
        IsRewardedAdCanceled = true;
    }

    #endregion

}
