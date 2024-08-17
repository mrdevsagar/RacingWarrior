using UnityEngine;
using UnityEngine.EventSystems;

public class Paddle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPaddleRight;
    public Drive drive;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPaddleRight)
        {
            drive._moveInput = 1;
        } else
        {
            drive._moveInput = -1;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPaddleRight && drive._moveInput == 1)
        {
            drive._moveInput = 0;
        }

        if (!isPaddleRight && drive._moveInput == -1)
        {
            drive._moveInput = 0;
        }
    }
}
