using UnityEngine;
using UnityEngine.UI;

public class AdvertisemetCanvas : MonoBehaviour
{
    [SerializeField] Button _loadBannerButton;
    [SerializeField] Button hideButton;
    [SerializeField] Button ShowButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _loadBannerButton.onClick.AddListener(LoadBanner);
        hideButton.onClick.AddListener(hideUnhideBanner);
        ShowButton.onClick.AddListener(ShowBanner);
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        AdMobsAds.Instance.canvasInstance.SetActive(!AdMobsAds.Instance.canvasInstance.activeSelf);
        AdMobsAds.Instance.LoadBannerAd();


    }
}
