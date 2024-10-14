// Ignore Spelling: Collider

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region State machine Reference
    public PlayerStateMachine PlayerStateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public PlayerInAirState InAirState { get; private set; }


    public WeaponStateMachine WeaponStateMachine { get; private set; }

    public WeaponFistState WeaponFistState { get; private set; }
    public WeaponGlovesState WeaponGlovesState { get; private set; }
    public WeaponSwordState WeaponSwordState { get; private set; }

    public WeaponPistolState WeaponPistolState { get; private set; }

    public WeaponRifleState WeaponRifleState { get; private set; }

    public WeaponBowState WeaponBowState { get; private set; }

    #endregion

    #region Unity Variables
    public Animator Anim { get; private set; }

    [SerializeField]
    private PlayerData _playerData;

    public PlayerInputHandler input;

    public Rigidbody2D RB { get; private set; }

    [SerializeField]
    public WeaponData WeaponData;


   
    public bool IsOverrideAnimation { get; private set; }

    [SerializeField]
    private Vector3 v;

    public Button JumpButton;

    public float maxPressTime = 0.01f;  // Maximum allowed press duration in seconds
    private float pressStartTime;      // When the button press started
    private bool isPressing = false;
    private bool isCancelled = false;  // Flag to track if the press is canceled

    #endregion

   

    #region Other Variables 

    public bool IsWalkingBackward;

    public bool IsPlayerLeftFacing = false;

    [SerializeField]
    private GameObject RightCineMachineCamera;
    [SerializeField]
    private GameObject LeftCineMachineCamera;
    [SerializeField]
    private GameObject BottomRightCineMachineCamera;
    [SerializeField]
    private GameObject BottomLeftCineMachineCamera;


    private List<GameObject> vertualCMCList;

    /// <summary>
    /// Components of Player and Weapons
    /// </summary>
    [SerializeField]public PlayerComponents Comp;

    private GameObject CurrentArrowObject;

    private bool IsArrowAvailable = false;

    private bool isFiring = false;
    private float customDistance = -0.5f;


    [Space(10)]


    public float headRotationAngle;

    public WeaponTypes SelectedWeapon = WeaponTypes.FIST;


    #endregion



    public bool CenterGrounded = false;

    public bool RightFootGrounded = false;
    public bool LeftFootGrounded = false;

    public float flipAngle = -180;

    #region Slope

    public Collider2D PlayerCollider;

    public List<GameObject> DisabledSlopeObjects;

    public PhysicsMaterial2D physicsMaterial;

    public Transform PlayerCameraHolder;
    #endregion

    #region Unity Callback functions


    private void Awake()
    {
        PlayerStateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, PlayerStateMachine, _playerData, "BodyIdle", "LegsIdle");
        MoveState = new PlayerMoveState(this, PlayerStateMachine, _playerData, "BodyWalk", "LegsWalk");
        InAirState = new PlayerInAirState(this, PlayerStateMachine, _playerData, "BodyJump", "LegsJump");


        WeaponStateMachine = new WeaponStateMachine();

        WeaponFistState = new WeaponFistState(this, PlayerStateMachine, _playerData, WeaponData,null);
        WeaponGlovesState = new WeaponGlovesState(this, PlayerStateMachine, _playerData, WeaponData,Comp.Gloves.GlovesGameObj);
        WeaponSwordState = new WeaponSwordState(this, PlayerStateMachine, _playerData, WeaponData,Comp.Sword.SwordGameObj);
        WeaponPistolState = new WeaponPistolState(this, PlayerStateMachine, _playerData, WeaponData,Comp.Pistol.PistolGameObj);
        WeaponRifleState = new WeaponRifleState(this, PlayerStateMachine, _playerData, WeaponData, Comp.Rifle.AkmGameObj);
        WeaponBowState = new WeaponBowState(this, PlayerStateMachine, _playerData, WeaponData,Comp.Bow.BowGameObj);

        input = GetComponent<PlayerInputHandler>();

        RB = GetComponent<Rigidbody2D>();

        DisabledSlopeObjects = new List<GameObject>();

        PlayerCollider.sharedMaterial = physicsMaterial;
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        //TODO : Initialize StateMachine

        PlayerStateMachine.Initialize(IdleState);
        WeaponStateMachine.Initialize(WeaponFistState);

        vertualCMCList = new List<GameObject>
        {
            RightCineMachineCamera,
            BottomRightCineMachineCamera,
            LeftCineMachineCamera,
            BottomLeftCineMachineCamera
        };

#if UNITY_ANDROID && !UNITY_EDITOR
                if (JumpButton != null)
                {
                    JumpButton.onClick.AddListener(OnJump);
                }
#endif
        
    }

    private void Update()
    {
        PlayerStateMachine.CurrentState.LogicUpdate();
        WeaponStateMachine.CurrentWeaponState.LogicUpdate();

        if (!input.LookInput.Equals(float.NaN))
        {
            ChangeCameraPosition(true);

            if (input.LookInput > 0.02f)
            {
                ManuallyCancelPress();
            }

        }

    }

    private void FixedUpdate()
    {
        PlayerStateMachine.CurrentState.PhysicsUpdate();
        WeaponStateMachine.CurrentWeaponState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        PlayerStateMachine.CurrentState.PhysicsLateUpdate();
        WeaponStateMachine.CurrentWeaponState.PhysicsLateUpdate();
    }

    private void OnJump()
    {
        PlayerStateMachine.CurrentState.OnJumpPress();
    }

    #endregion


    #region Important Functions
    

    public bool IsTouchingGround()
    {
        return CenterGrounded || RightFootGrounded || LeftFootGrounded;
    }

    #endregion
    #region Other Public Methods 
    public void SetVelocityX(float velocityX)
    {
        RB.velocity = new Vector2(velocityX, RB.velocity.y);
    }

   


    public void FlipPlayer(bool isRightFacing)
    {
        IsPlayerLeftFacing = !isRightFacing;
        if (isRightFacing)
        {
            transform.localScale = new Vector3(0.5f, 0.5f,0f);
        } else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0f);
        }
        ChangeCameraPosition(isRightFacing);
    }

    public void SwordMovement()
    {
        if (input.IsFiring)
        {
            AttackSword();
        } 
    }

    public void AttackSword()
    {
        PlayerStateMachine.CurrentState.SetCurrentBodyAnimation(false);
        Anim.SetBool("SordAttck", true);
    }

  
    public GameObject InstantiateArrow(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    public float ConvertValue(float value, Vector2 inputRange, Vector2 outputRange)
    {
        // Linear interpolation formula
        float outputValue = outputRange.x + (value - inputRange.x) * (outputRange.y - outputRange.x) / (inputRange.y - inputRange.x);

        return outputValue;
    }
    public void RotateHead(float angle)
    {
        if (angle >= 0 && angle <= 90)
        {
            headRotationAngle = 95f + angle / 3;
        }
        else if (angle >= 270 && angle <= 360)
        {
            /*headRotationAngle = 95f - (360 - angle) / 3;*/
            headRotationAngle = 95f - (35f * (360 - angle) / 90);
        }

        if (angle > 90 && angle < 270)
        {
            headRotationAngle = -(125f - (65f * (angle - 90) / 180)); // Transition from 125 to 60
        }

       

        Comp.Body.Head.eulerAngles = new Vector3(Comp.Body.Head.eulerAngles.x, Comp.Body.Head.eulerAngles.y, headRotationAngle);

        
    }

    public void MoveBowRightHand(float angle,float distance)
    {
        
    }

    public void OnImportantAnimationStarts()
    {
        IsOverrideAnimation = true;
    }

    public void OnImportantAnimationDone(string BodyAnimationBoolName)
    {
        IsOverrideAnimation = false;
        Anim.SetBool(BodyAnimationBoolName, false);
        PlayerStateMachine.CurrentState.SetCurrentBodyAnimation(true);
    }

    #endregion

    #region FireButtton
    // When the button is pressed down
    public void OnPointerDown()
    {
        pressStartTime = Time.time;  // Record the time when button is pressed
        isPressing = true;           // Set the pressing flag to true
        isCancelled = false;         // Reset the cancel flag
    }

    // When the button is released
    public void OnPointerUp()
    {
        if (isPressing && !isCancelled)
        {
            float pressDuration = Time.time - pressStartTime;  // Calculate how long the button was pressed

            // Check if the button was pressed and released quickly (within the allowed time)
            if (pressDuration <= maxPressTime)
            {
                OnQuickPress();  // Call the function for quick press
            }
            else
            {
                CancelPress();  // Call the function to cancel the press if it exceeds maxPressTime
            }

            isPressing = false;  // Reset the pressing flag
        }
    }

    // This function will be called if the button is pressed and released within the allowed time
    private void OnQuickPress()
    {
        Debug.Log("Button pressed and released within the time limit!");
        // Add your functionality here for quick press

        /*AttackSword();*/

        WeaponStateMachine.CurrentWeaponState.AttackWeapon();
    }

    // This function will be called if the button press is canceled (e.g., too long or manual cancel)
    private void CancelPress()
    {
        Debug.Log("Button press was canceled!");
        // Add your functionality here for press cancelation
        isCancelled = true;  // Set the flag to true to prevent further actions
    }

    // Manual cancel function (you can call this method from anywhere in your code to cancel the press)
    public void ManuallyCancelPress()
    {
        if (isPressing)
        {
            CancelPress();
            isPressing = false;
        }
    }
    #endregion

    #region Private Methods
    private void ChangeCameraPosition(bool isRightFacing)
    {
        return;

        float angle = input.LookInput;


        if ((angle >= 0 && angle < 90) || (angle >= 350 && angle < 360))
        {
            EnableSelectedVCMC(RightCineMachineCamera);
        }
        else if (angle >= 90 && angle < 190)
        {
            EnableSelectedVCMC(LeftCineMachineCamera);
        }
        else if (angle >= 190 && angle < 270)
        {
            EnableSelectedVCMC(BottomLeftCineMachineCamera);
        }
        else if (angle >= 270 && angle < 350)
        {
            EnableSelectedVCMC(BottomRightCineMachineCamera);
        }
        else
        {
            RightCineMachineCamera.SetActive(isRightFacing);
            LeftCineMachineCamera.SetActive(!isRightFacing);
            BottomRightCineMachineCamera.SetActive(false);
            BottomLeftCineMachineCamera.SetActive(false);
        }

        void EnableSelectedVCMC(GameObject toEnable)
        {

            foreach (GameObject obj in vertualCMCList)
            {
                if (obj == toEnable)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }

    }

    public void SwitchWeapon()
    {
            SelectedWeapon++;
            
            if(SelectedWeapon == WeaponTypes.SWORD )
            {
                WeaponStateMachine.ChangeState(WeaponSwordState);
            } 
            else if (SelectedWeapon == WeaponTypes.AKM)
            {
                WeaponStateMachine.ChangeState(WeaponRifleState);
            } 
            else if  (SelectedWeapon == WeaponTypes.BOW) {
                WeaponStateMachine.ChangeState(WeaponBowState);
            }
            else if (SelectedWeapon == WeaponTypes.Revolver)
            {
                WeaponStateMachine.ChangeState(WeaponPistolState);
            }
            else if (SelectedWeapon == WeaponTypes.GLOWS)
            {
                WeaponStateMachine.ChangeState(WeaponGlovesState);
            }
            else
            {
                WeaponStateMachine.ChangeState(WeaponFistState);
            }

            // If we've gone past the last weapon state, loop back to the first one
            if ((int)SelectedWeapon >= System.Enum.GetValues(typeof(WeaponTypes)).Length)
            {
                SelectedWeapon = WeaponTypes.FIST;
            } 
    }

    #endregion

    #region Slope Go through
    public void DisableCollisionForPlayer(Collider2D collider)
    {
       
        Physics2D.IgnoreCollision(collider, PlayerCollider);
 
    }

    public void EnableCollisionForPlayer(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, PlayerCollider, false);
    }

    #endregion
}


//4.33 0.65