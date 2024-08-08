using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DCTBuyPanelController : MonoBehaviour
{
    [SerializeField] BuyCard crate;
    [SerializeField] BuyCard diamondReward;
    [SerializeField] BuyCard coinReward;
    [SerializeField] BuyCard cashReward;

    private void Start()
    {
        coinReward.btnText.text = "afkaskdjfhkja";
    }

    private void Update()
    {
        if(AdMobsAds.Instance.CountDownValue > 0)
        {
            diamondReward.btnText.text = AdMobsAds.Instance.CountDownValue.ToString();
            diamondReward.btn.interactable = false;
            coinReward.btnText.text = AdMobsAds.Instance.CountDownValue.ToString();
            coinReward.btn.interactable = false;
            cashReward.btnText.text = AdMobsAds.Instance.CountDownValue.ToString();
            cashReward.btn.interactable = false;
        } else
        {
            if (AdMobsAds.Instance != null && AdMobsAds.Instance.IsLoadingRewardedAd)
            {
                diamondReward.btnText.text = "Loading.";
                diamondReward.btn.interactable = false;
                coinReward.btnText.text = "Loading.";
                coinReward.btn.interactable = false;
                cashReward.btnText.text = "Loading.";
                cashReward.btn.interactable = false;
            }
            else
            {
                diamondReward.btnText.text = "Watch Ad";
                diamondReward.btn.interactable = true;
                coinReward.btnText.text = "Watch Ad";
                coinReward.btn.interactable = true;
                cashReward.btnText.text = "Watch Ad";
                cashReward.btn.interactable = true;
            }
        }
    }

    public void RequestAddForDiamond()
    {
       RequestRewardedAd("Congresses", "You Got A Diamond.", 1, Collectible.DIAMOND);
    }

    public void RequestAddForCoin()
    {
        RequestRewardedAd("Congresses", "You Got 2000 coins", 2000, Collectible.COIN);
    }
    public void RequestAddForToken()
    {
        RequestRewardedAd("Congresses", "You Got 50 Tokens", 50, Collectible.TOKEN);
    }
    

    private void RequestRewardedAd(string title, string subTitle, int count, Collectible collectibleType)
    {
        AdManagerAI.Instance.LoadAndOrShowRewardedVideoAd(title, subTitle, count, collectibleType);
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;

        if (pointerData != null && ((diamondReward != null && !RectTransformUtility.RectangleContainsScreenPoint(diamondReward.GetComponent<RectTransform>(), pointerData.position, null)) 
            &&
            (diamondReward != null && !RectTransformUtility.RectangleContainsScreenPoint(diamondReward.GetComponent<RectTransform>(), pointerData.position, null))
            &&
            (diamondReward != null && !RectTransformUtility.RectangleContainsScreenPoint(diamondReward.GetComponent<RectTransform>(), pointerData.position, null)))
            )
        {
            // Close the menu if the click is outside the menu panel
            AdManagerAI.Instance.CancelShowingRewardedAd();
        }
    }
}
