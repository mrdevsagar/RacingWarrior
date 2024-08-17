using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class DCTBuyPanelController : MonoBehaviour
{
  
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

    public void RequestAdForDiamond()
    {
       RequestRewardedAd("Congresses", "You Got 2 Diamonds.", 2, Collectible.DIAMOND);
    }

    public void RequestAdForCoin()
    {
        RequestRewardedAd("Congresses", "You Got 10000 coins", 10000, Collectible.COIN);
    }
    public void RequestAdForToken()
    {
        RequestRewardedAd("Congresses", "You Got 100 Tokens", 100, Collectible.TOKEN);
    }


    public void SpendForCoin()
    {
        if(GameManager.Instance.SpendDiamonds(3))
        {
            AdMobsAds.Instance.ShowRewardPanel("Coins", "You Got 10,000 coins", 10000, Collectible.COIN);
            GameManager.Instance.AddCoins(10000);
        } else
        {
            NotEnoughDiamond();
        }
        
    }
    public void SpendForToken()
    {
        //50 tokens
        if (GameManager.Instance.SpendDiamonds(1))
        {
            TokenManager.Instance.AddToken(50);
            AdMobsAds.Instance.ShowRewardPanel("Tokens", "You Got 50 Tokens", 50, Collectible.TOKEN);
        }
        else
        {
            NotEnoughDiamond();
        }
    }


    public void SpendForCoin2()
    {
        if (GameManager.Instance.SpendDiamonds(5))
        {
            AdMobsAds.Instance.ShowRewardPanel("Coins", "You Got 25,000 coins", 25000, Collectible.COIN);
            GameManager.Instance.AddCoins(25000);
        }
        else
        {
            NotEnoughDiamond();
        }
    }
    public void SpendForToken2()
    {
        if (GameManager.Instance.SpendDiamonds(3))
        {
            //200 tokens
            AdMobsAds.Instance.ShowRewardPanel("Tokens", "You Got 200 Tokens", 200, Collectible.TOKEN);
            TokenManager.Instance.AddToken(200);
        }
        else
        {
            NotEnoughDiamond();
        }
    }

    private void NotEnoughDiamond()
    {
        AdMobsAds.Instance.ShowRewardPanel("Warning", "Not Enough Diamond", 0, Collectible.NO_DIAMOND);
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

  /*  public void ShowRewardPanel(string title, string subTitle, int count, Collectible collectibleType)
    {
        var rewardCanvas = FindFirstObjectByType<RewardCanvas>();
        GameObject myItem = null;
        if (rewardCanvas != null)
        {
            myItem = rewardCanvas.gameObject;
        } 
        if (myItem == null)
        {

            GameObject prefab = Resources.Load<GameObject>("RewardCanvas");
            myItem = Instantiate(prefab) as GameObject;
        }
        
        if(myItem != null)
        {
            myItem.GetComponent<RewardCanvas>().ShowCanvas(title, subTitle, count, collectibleType);
        }
    }
*/
  
}
