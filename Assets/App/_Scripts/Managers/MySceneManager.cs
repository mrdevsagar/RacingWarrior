using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    [SerializeField]
    private bool isAGameView  = false;

    [SerializeField]
    private bool showBannerOnScreenLoad= false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.Instantiate();
        AdManagerAI.Instance.Instantiate();
        TokenManager.Instance.Instantiate();
        MyLoadSceneAsync.Instance.Instantiate();

        GameManager.Instance.PauseGame(false);

        if (isAGameView)
        {
            AdManagerAI.Instance.EnterGameView();
        } else
        {
            AdManagerAI.Instance.ExitGameView();
        }

        if (showBannerOnScreenLoad)
        {
            Debug.Log("isAGameView" + showBannerOnScreenLoad);
            AdManagerAI.Instance.ShowBannerAd();
        }
        else
        {
            AdManagerAI.Instance.HideBannerAd();
        }

    }

    
}
