using UnityEngine;
using UnityEngine.EventSystems;

public class OutsideClickHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // Reference to the panel

    void Start()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Menu Panel is not assigned in the inspector.");
        }
    }

    public void OnPointerClick(BaseEventData data)
    {
        Debug.Log("............................................");
        PointerEventData pointerData = data as PointerEventData;

        if (pointerData != null && menuPanel != null && !RectTransformUtility.RectangleContainsScreenPoint(menuPanel.GetComponent<RectTransform>(), pointerData.position, null))
        {
            // Close the menu if the click is outside the menu panel
            menuPanel.SetActive(false);
        }
    }
}
