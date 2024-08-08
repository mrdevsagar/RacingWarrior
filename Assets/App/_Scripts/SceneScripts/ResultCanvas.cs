using UnityEngine;
using UnityEngine.UI;
public class ResultCanvas : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private SceneField nextLevelScene;
    void Awake()
    {
        continueButton.onClick.AddListener(OnContinueNextLevel);
        if (LevelManager.Instance.IsPlayerWon)
        {
            continueButton.enabled = true;
        }
    }

    // This method will be called when the button is clicked
    public void OnContinueNextLevel()
    {       
            MyLoadSceneAsync.Instance.Load(nextLevelScene);
    }
}
