using UnityEngine;
using UnityEngine.UI;

public class AdvertisemetCanvas : MonoBehaviour
{
    [SerializeField] Button _loadBannerButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _loadBannerButton.onClick.AddListener(LoadBanner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadBanner()
    {
        AdMobsAds.Instance.canvasInstance.SetActive(!AdMobsAds.Instance.canvasInstance.activeSelf);
        AdMobsAds.Instance.LoadBannerAd();
    }
}
