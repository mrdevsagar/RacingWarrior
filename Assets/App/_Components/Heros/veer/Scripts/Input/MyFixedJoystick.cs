using UnityEngine;
using UnityEngine.EventSystems;

public class MyFixedJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 lastPosition; // Store the last position of the joystick knob
    [SerializeField]
    private RectTransform knobRectTransform; // Reference to the joystick knob
    [SerializeField]
    private float radius = 50f; // Maximum distance from the center to the knob
  
    private bool isFiring; // Flag to indicate whether firing is active
    private float distance;
    private float angle;
/*
    public float Distance { get => distance;private set => distance = value; }
    public float Angle { get => angle;private set => angle = value; }
    public bool IsFiring { get => isFiring; set => isFiring = value; }*/

    public float Distance
    {
        get => distance;
        private set
        {
            if (distance != value)
            {
                distance = value;
                JoystickEventManager.TriggerDistanceChanged(distance);
            }
        }
    }

    public float Angle
    {
        get => angle;
        private set
        {
            if (angle != value)
            {
                angle = value;
                JoystickEventManager.TriggerAngleChanged(angle);
            }
        }
    }

    public bool IsFiring
    {
        get => isFiring;
        set
        {
            if (isFiring != value)
            {
                isFiring = value;
                JoystickEventManager.TriggerFiringChanged(isFiring);
            }
        }
    }

    private void Start()
    {
        lastPosition = knobRectTransform.anchoredPosition; // Initialize with the starting position
/*        radius = GetComponent<RectTransform>().anchoredPosition.x / 2;*/
        IsFiring = false; // Initialize firing to false
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            knobRectTransform.parent as RectTransform, // The parent RectTransform
            eventData.position,                        // The screen position of the touch
            eventData.pressEventCamera,                // The camera used to convert the screen point
            out localPoint                             // Output the local position
        );

        // Calculate the new position of the joystick knob based on the local point
        Vector2 newPosition = localPoint;

        // Clamp the position within a certain radius
        if (newPosition.magnitude >= radius)
        {
            newPosition = newPosition.normalized * radius;
            IsFiring = true; // Set firing if touched outside radius
        }
        else
        {
            IsFiring = false;
        }

        // Log the distance and angle before updating the knob's position
        LogDistanceAndAngle(newPosition);

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        lastPosition = newPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsFiring = false; // Reset firing when the touch is released
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Player is touching the knob
      

        // Convert the screen touch position to the local position relative to the joystick's parent (usually a canvas)
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            knobRectTransform.parent as RectTransform, // The parent RectTransform
            eventData.position,                        // The screen position of the touch
            eventData.pressEventCamera,                // The camera used to convert the screen point
            out localPoint                             // Output the local position
        );

        // Calculate the new position of the joystick knob based on the local point
        Vector2 newPosition = localPoint;

        // Clamp the position within a certain radius
        if (newPosition.magnitude >= radius)
        {
            newPosition = newPosition.normalized * radius;
            IsFiring = true; // Set firing if touched outside radius
        }

        // Log the distance and angle before updating the knob's position
        LogDistanceAndAngle(newPosition);

        // Update the knob's position
        knobRectTransform.anchoredPosition = newPosition;

        lastPosition = newPosition;
    }

    private void LogDistanceAndAngle(Vector2 position)
    {
        // Calculate distance from the center
         Distance = ConvertValue(position.magnitude, new Vector2(0,radius),new Vector2(0,1));

        // Calculate angle in degrees and normalize to [0, 360]
         float  tempAngle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        Angle = (tempAngle + 360) % 360; // Ensure the angle is positive

        // Log the distance and angle
       
    }

    public float ConvertValue(float value, Vector2 inputRange, Vector2 outputRange)
    {
        // Linear interpolation formula
        float outputValue = outputRange.x + (value - inputRange.x) * (outputRange.y - outputRange.x) / (inputRange.y - inputRange.x);

        return outputValue;
    }
    public void Update()
    {
       /* Debug.Log(IsFiring +"  "+ Distance + "  " + Angle);*/
    }
    public void ResetJoystick()
    {
        // Optional: Call this method to reset the joystick back to the center
        lastPosition = Vector2.zero; // Reset last position to zero
        knobRectTransform.anchoredPosition = Vector2.zero; // Reset knob to center
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: Implement any logic for when dragging ends
    }
}
