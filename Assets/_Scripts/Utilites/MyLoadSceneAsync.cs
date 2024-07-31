using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MyLoadSceneAsync : Singleton<MyLoadSceneAsync>
{
    [SerializeField]
    private GameObject loadCanvas;
    [SerializeField]
    private GameObject loaderPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        loaderPrefab = Resources.Load<GameObject>("RewardCanvas");
        loadCanvas = Instantiate(loaderPrefab) as GameObject;
        loadCanvas.SetActive(false);
        loadCanvas.transform.parent = gameObject.transform;
    }


    public void Load(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Sooraj: Scene name should not be empty");
            return;
        }
        ShowLoadingScreen();
        StartCoroutine(LoadYourScene(sceneName));
    }

    public void HideLoadingScreen()
    {
        
        loadCanvas.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadCanvas.GetComponent<RewardCanvas>().ShowCanvas("Loading");
        loadCanvas.SetActive(true);
    }

    private IEnumerator LoadYourScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if(asyncLoad == null)
        {
            Debug.Log("exited");
            HideLoadingScreen();
            yield  break;
        }

        asyncLoad.allowSceneActivation = false;
        // While the scene is loading, you can perform other operations
        while (!asyncLoad.isDone)
        {

            if (asyncLoad.progress >= 0.9f)
            {
                /*yield return new WaitForSeconds(0f);*/
                
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        HideLoadingScreen();
    }
}
