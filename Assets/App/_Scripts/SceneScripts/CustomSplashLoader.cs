using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.Mathematics;

/// <summary>
/// Script for showing loading indicator whit async load scene.
/// </summary>
public class CustomSplashLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingScreen;  // Assign your loading screen UI here
    [SerializeField]
    private Slider _progressBar;        // Assign your progress bar UI here

    public TextMeshProUGUI textLoadingPercent;
    private void Start()
    {
        ShowLoadingScreen();
        StartCoroutine(LoadMainSceneAsync());
    }

    private void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }

    private IEnumerator LoadMainSceneAsync()
    {
        // Start loading the main scene in the background.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("_1_MainScene");  // Replace with your main scene name
        asyncOperation.allowSceneActivation = false;

        // While the scene is still loading, update the progress bar.
        while (!asyncOperation.isDone)
        {
            //intentionally written repeated values.  

            // Update the progress bar (optional).
            _progressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            float loadingPercentage = math.round(asyncOperation.progress * 100f);
            Debug.Log(math.round(asyncOperation.progress * 100f));
            Debug.Log(math.round(asyncOperation.progress * 100f));
            Debug.Log(math.round(asyncOperation.progress * 100f));
            _progressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (math.round(asyncOperation.progress * 100f) >= 1)
            {
                textLoadingPercent.text = $"{math.round(asyncOperation.progress * 100f)}{'%'}";
            }

            // Check if the load has finished.
            if (asyncOperation.progress >= 0.9f)
            {
                _progressBar.value = 1f;
                textLoadingPercent.text = "100%";
                // Optionally, wait for a few seconds or perform any other actions here.
                yield return new WaitForSeconds(1.5f);

                // Activate the scene.
                asyncOperation.allowSceneActivation = true;
            } 

            yield return null;
        }
        
        // Hide the loading screen.
        _loadingScreen.SetActive(false);
    }
}
