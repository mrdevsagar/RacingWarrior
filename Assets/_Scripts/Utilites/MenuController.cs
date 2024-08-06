using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button openButton; // Button to toggle the menu

    private GameObject menuPanel;
    

    private void Start()
    {
        menuPanel = gameObject; // Attach the script to the MenuPanel GameObject

        if (openButton == null)
        {
            Debug.LogError("OpenButton is not assigned in the inspector.");
            return;
        }

        // Initially hide the menu panel
        menuPanel.SetActive(false);

        // Add listener to the button
        openButton.onClick.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
}
