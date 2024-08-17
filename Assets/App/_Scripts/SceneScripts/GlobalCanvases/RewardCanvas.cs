using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RewardCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI headingText;

    [SerializeField] Image CollectableImage;

    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] List<Sprite> shapes;

    [SerializeField] GameObject rewardCanvas;



    // Start is called once before the first execution of Update after the MonoBehaviour is created




    void Start()
    {
        
    }

    public void ShowCanvas(string title, string subTitle, int count, Collectible collectibleType)
    {
        rewardCanvas.SetActive(true);
       

        headingText.text = title;
        descriptionText.text = subTitle;
        Sprite image = null;
        switch (collectibleType)
        {
            case Collectible.DIAMOND:
                image = shapes[0];
                break;
            case Collectible.COIN:
                image = shapes[1];
                break;
            case Collectible.TOKEN:
                image = shapes[2];
                break;

            case Collectible.NO_DIAMOND:
                image = shapes[3];
                break;


            default:
                break;
        }

        if(image != null)
        {
            CollectableImage.sprite = image;
        }
    }

    public void HideCanvas()
    {
        rewardCanvas.SetActive(false);
    }

}

