using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject Scrollbar;
    float _scollPos = 0f;
    float[] _pos;
    float _distance = 0;

    bool _isTouchedToScreen = false;

    private PlayerInput playerInput;

    private InputAction _touchPressAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _touchPressAction = playerInput.actions["TouchPress"];
    }
    private void Start()
    {
        _pos = new float[transform.childCount];
         _distance = 1f / (_pos.Length - 1f);

        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = _distance * i;
        }
    }

    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
        _touchPressAction.canceled += TouchPressed;
    }

    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
        _touchPressAction.canceled += TouchPressed;
    }

    private void Update()
    {
        if (_isTouchedToScreen)
        {
            _scollPos = Scrollbar.GetComponent<Scrollbar>().value;
        } else 
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scollPos < _pos[i] + (_distance/2) && _scollPos > _pos[i] - (_distance / 2))
                {
                    Scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(Scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                }
            }
        }

        for (int i=0; i < _pos.Length;i++)
        {
            if (_scollPos < _pos[i] + (_distance / 2) && _scollPos > _pos[i] - (_distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
            } else
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.7f, 0.7f), 0.1f);
            }
        }

    }

    public void TouchPressed(InputAction.CallbackContext context)
    {
        if(_touchPressAction.ReadValue<float>()>0)
        {
            _isTouchedToScreen = true;
        } else
        {
            _isTouchedToScreen = false;
        }
       
    }

}
