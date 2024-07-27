using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCanvas : MonoBehaviour
{
    public GameObject gameObject;
    [SerializeField] TextMeshProUGUI tokensText;

    public float delayInSeconds = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ShowCanvas(string title)
    {
        gameObject.SetActive(true);
        StartCoroutine(CallFunctionAfterDelay());
        if(tokensText != null) {
            tokensText.text = title;
        }


    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator CallFunctionAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        HideCanvas();
    }
}

