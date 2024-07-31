using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchSceneAsyncButton : MonoBehaviour
{
    [SerializeField]
    private SceneField sceneName;

    private Button button;

    void Awake()
    {
        // Get the Button component attached to the same GameObject
        button = GetComponent<Button>();

        // Add an OnClick listener
        button.onClick.AddListener(OnButtonClick);
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
        MyLoadSceneAsync.Instance.Load(sceneName);
    }

    void OnDestroy()
    {
        // Remove the listener when the object is destroyed to prevent memory leaks
        button.onClick.RemoveListener(OnButtonClick);
    }
}
