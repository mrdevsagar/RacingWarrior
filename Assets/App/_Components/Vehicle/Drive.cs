// Ignore Spelling: collider

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _frontTireRB;
    [SerializeField] private Rigidbody2D _backTireRB;
     public Rigidbody2D VehicleRB;

    [SerializeField] private float _speed = 2000f;
    [SerializeField] private float _rotationalSpeed = 300f;

    [SerializeField] Collider2D CarBody;
    [SerializeField] Collider2D WheelRight;
    [SerializeField] Collider2D WheelLeft;

    public PlayerInputHandler input;

    public float _moveInput;

    [SerializeField]
    public List<GameObject> DisabledSlopeObjects;

   

    private Button upButton;
    private Button downButton;

    [SerializeField]
    private Transform WeaponHolder;

    
    private void Awake()
    {
        DisabledSlopeObjects = new List<GameObject>();
    }


    private void OnEnable()
    {
        // Subscribe to the rotation event
        CanvasController.OnRotationChanged += RotateObject;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        CanvasController.OnRotationChanged -= RotateObject;
    }

    public void RotateObject(CanvasController.RotationDirection direction)
    {

        switch (direction)
        {
            case CanvasController.RotationDirection.Left:
                _moveInput = -1;
                break;
            case CanvasController.RotationDirection.Right:
                _moveInput = 1;
                break;
            case CanvasController.RotationDirection.None:
                _moveInput = 0;
                break;
        }
    }
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _frontTireRB.AddTorque(-input.MoveInput.x * _speed * Time.fixedDeltaTime);
        _backTireRB.AddTorque(-input.MoveInput.x * _speed * Time.fixedDeltaTime);
        VehicleRB.AddTorque(_moveInput * _rotationalSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        VehicleWeaponAim();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "End")
        {
            //Call Game Over
            Debug.Log("Win");
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.GameOver();
    }


    public void DisableCollisionForVehicle(Collider2D collider)
    {
        /*if (collider.gameObject.tag == "SlopeRight" || collider.gameObject.tag == "SlopeLeft")
        {*/
            Physics2D.IgnoreCollision(collider, CarBody);
            Physics2D.IgnoreCollision(collider, WheelLeft);
            Physics2D.IgnoreCollision(collider, WheelRight);
        /*}*/
    }

    public void EnableCollisionForVehicle(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, CarBody, false);
        Physics2D.IgnoreCollision(collider, WheelLeft, false);
        Physics2D.IgnoreCollision(collider, WheelRight, false);
    }

    private void VehicleWeaponAim()
    {
        float angle = input.LookInput;

        float handRotationAngle = angle;



        if (!handRotationAngle.Equals(float.NaN))
        {
            WeaponHolder.eulerAngles = new Vector3(WeaponHolder.rotation.x, WeaponHolder.rotation.y, handRotationAngle);
        }
    }
}
