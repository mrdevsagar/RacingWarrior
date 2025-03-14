// Ignore Spelling: Collider

using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEditor;
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


    public WeaponStateMachine WeaponStateMachine;

    public WeaponFistState WeaponFistState { get; private set; }
    public WeaponGlovesState WeaponGlovesState { get; private set; }
    public WeaponSwordState WeaponSwordState { get; private set; }


    public WeaponPistolState WeaponPistolState;

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

    public float maxPressTime = 0.01f;  // Maximum allowed press duration in seconds
    private float pressStartTime;      // When the button press started
    private bool isPressing = false;
    private bool isCancelled = false;  // Flag to track if the press is canceled

    private float _curretHelath;
    private float _maxHealth = 100f;
    #endregion



    #region Other Variables 

    public bool IsWalkingBackward;

    public bool IsPlayerLeftFacing = false;


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

    public GameObject VehicleNearBy;

    public List<GameObject> VehicleNearByList;

    private CapsuleCollider2D _capsuleCollider;

   /* [TagSelectorDropdown]*/
    private string ExternalVehicleColliderTag = "ExternalColliderVehicle";

    public bool IsDead =false;

    #endregion

    #region Unity Callback functions


    public IKManager2D _iKManager;

    public bool isDemo;

    [Tooltip("Add GameObjects with JointData components here.")]
    public List<JointData> jointDataList = new List<JointData>();

    private HealthBar _healthBar;

    public float slopeAngle;

    public LayerMask groundLayer; // Layer to detect for slope
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

        CanvasController.SwitchControls(true);

        _curretHelath = _maxHealth;

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        //TODO : Initialize StateMachine

        PlayerStateMachine.Initialize(IdleState);
        WeaponStateMachine.Initialize(WeaponFistState);

        CameraSwitcher.TriggerSwitchToPlayer(gameObject);
        _capsuleCollider = GetComponent<CapsuleCollider2D>();


        foreach (JointData jointData in jointDataList)
        {

            JointAngleLimits2D jointLimit = new JointAngleLimits2D();
            jointLimit.min = jointData.vectorA.x;
            jointLimit.max = jointData.vectorA.y;
            jointData.hingeJoint.limits = jointLimit;

        }

        _healthBar = GetComponentInChildren<HealthBar>();
    }

    private void OnEnable()
    {
        /*Anim.enabled = true;*/
        CanvasController.SwitchControls(true);
      /*  ResetCollider(_capsuleCollider);*/

       /* Anim.speed = 1;*/

      
        if (!isDemo)
        {
            // For layer 0
            AnimatorStateInfo stateInfoLayer0 = Anim.GetCurrentAnimatorStateInfo(0);

            // For layer 1
            AnimatorStateInfo stateInfoLayer1 = Anim.GetCurrentAnimatorStateInfo(1);

            Anim.Play(stateInfoLayer0.fullPathHash, 0, 0f); // Start from the beginning
            Anim.Play(stateInfoLayer1.fullPathHash, 1, 0f);
        }
        

        
    }

    private void OnDisable()
    {
       

        float endOfAnimation = 0f;

    /*    // For layer 0
        AnimatorStateInfo stateInfoLayer0 = Anim.GetCurrentAnimatorStateInfo(0);
        Anim.Play(stateInfoLayer0.fullPathHash, 0, endOfAnimation);

        // For layer 1
        AnimatorStateInfo stateInfoLayer1 = Anim.GetCurrentAnimatorStateInfo(1);
        Anim.Play(stateInfoLayer1.fullPathHash, 1, endOfAnimation);

        Anim.speed = 0;

        Anim.enabled = false;*/

        CanvasController.SwitchControls(false);
       /* ResetCollider(_capsuleCollider);*/
    }

    private void ResetCollider(CapsuleCollider2D capsuleCollider)
    {
        if (capsuleCollider != null)
        {
           
            // Set the size (width, height) of the CapsuleCollider2D
            capsuleCollider.size = new Vector2(0.9f, 3.02f); // Example values for size

            // Set the offset of the CapsuleCollider2D
            capsuleCollider.offset = new Vector2(0, 1.06f); // Example values for offset
        }
        else
        {
            Debug.LogWarning("No CapsuleCollider2D found on this GameObject.");
        }
    }

    private void Update()
    {
        DetectSlope();
        PlayerStateMachine.CurrentState.LogicUpdate();
        WeaponStateMachine.CurrentWeaponState.LogicUpdate();


        

        if (!input.LookInput.Equals(float.NaN))
        {
            if (input.LookInput > 0.02f)
            {
                ManuallyCancelPress();
            }

        }

        
    }

    private void FixedUpdate()
    {
        RotatePlayer();
        PlayerStateMachine.CurrentState.PhysicsUpdate();
        WeaponStateMachine.CurrentWeaponState.PhysicsUpdate();
        
    }

    private void LateUpdate()
    {/*
        Debug.Log(WeaponStateMachine);*/
        PlayerStateMachine.CurrentState.PhysicsLateUpdate();
        WeaponStateMachine.CurrentWeaponState.PhysicsLateUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        
        if (collision.gameObject.CompareTag(ExternalVehicleColliderTag))
        {
           /* Debug.LogWarning(collision.gameObject.tag);*/
            if (VehicleNearByList.Contains(collision?.gameObject?.transform?.parent?.gameObject?.transform?.parent?.gameObject)) { 
            
            }
            else
            {
                VehicleNearByList.Add(collision?.gameObject?.transform?.parent?.gameObject?.transform?.parent?.gameObject);
            }
        }

        if (VehicleNearByList.Count > 0)
        {
            CanvasController.IsPlayerNearVehicle(true);
        } else
        {
            CanvasController.IsPlayerNearVehicle(false);
        }
    }

    public void GetInsideVehicle()
    {
        if (VehicleNearByList.Count >0)
        {
            VehicleNearByList[0].GetComponent<Drive>().DriveVehicle(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag(ExternalVehicleColliderTag))
        {
            VehicleNearByList.Remove(collision?.gameObject?.transform?.parent?.gameObject?.transform?.parent?.gameObject);
        }
*/
        if (VehicleNearByList.Count == 0)
        {
            CanvasController.IsPlayerNearVehicle(false);
        }
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
        RB.linearVelocity = new Vector2(velocityX, RB.linearVelocity.y);
    }

   


    public void FlipPlayer(bool isRightFacing)
    {
        IsPlayerLeftFacing = !isRightFacing;

        foreach (JointData jointData in jointDataList)
        {
            jointData.hingeJoint.useLimits = false;
            JointAngleLimits2D jointLimit = jointData.hingeJoint.limits;
            Vector2 angle = jointData.vectorA;
            if (IsPlayerLeftFacing)
            {
                angle.x = jointData.vectorB.x;
                angle.y = jointData.vectorB.y;
            }
            jointLimit.min = angle.x;
            jointLimit.max = angle.y;

            jointData.hingeJoint.limits = jointLimit;
            jointData.hingeJoint.useLimits = true;

        }

        if (isRightFacing)
        {
            transform.localScale = new Vector3(0.5f, 0.5f,0f);
        } else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0f);
        }
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

       
        if (!IsDead)
        {
            Comp.Body.Head.eulerAngles = new Vector3(Comp.Body.Head.eulerAngles.x, Comp.Body.Head.eulerAngles.y, headRotationAngle);
        }
        

        
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
    
    public void SwitchWeapon()
    {
            SelectedWeapon++;

        Debug.Log(SelectedWeapon);
        Debug.Log(PlayerStateMachine);
        Debug.Log(WeaponStateMachine);
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


    public void ActivateRagdoll()
    {
        _iKManager.weight = 0;

        foreach (JointData jointData in jointDataList)
        {

            /*Rigidbody2D jointRB = jointData.hingeJoint.gameObject.GetComponent<Rigidbody2D>();


            JointAngleLimits2D jointLimit = jointData.hingeJoint.limits;
            jointLimit.min = IsPlayerLeftFacing ? jointData.vectorB.x : jointData.vectorA.x;
            jointLimit.max = IsPlayerLeftFacing ? jointData.vectorB.y : jointData.vectorA.y;

            jointData.hingeJoint.limits = jointLimit;
            jointData.hingeJoint.useLimits = true;*/
            

            

           /* jointRB.simulated = true;*/
        }



      /*  foreach (JointData jointData in jointDataList)
        {

            Rigidbody2D jointRB = jointData.hingeJoint.gameObject.GetComponent<Rigidbody2D>();

            jointData.vectorA.x = jointData.hingeJoint.limits.min;
            jointData.vectorA.y = jointData.hingeJoint.limits.max;
        }*/
    }


    public void Tigger()
    {
        _iKManager.weight = 0;
        IsDead = true;
        foreach (JointData jointData in jointDataList)
        {
            Rigidbody2D jointRB = jointData.hingeJoint.gameObject.GetComponent<Rigidbody2D>();
            jointRB.isKinematic = false;
        }
    }
    public void DeactivateRegdoll()
    {
        foreach (JointData jointData in jointDataList)
        {
            jointData.hingeJoint.gameObject.GetComponent<Rigidbody2D>().isKinematic =true;
        }
    }

    private Vector2 CalculateFlippedAngles(Vector2 vectorA)
    {
        // Step 1: Negate the original lower and upper angles
        float invertedLower = -vectorA.y; // Invert upper to become the new lower
        float invertedUpper = -vectorA.x; // Invert lower to become the new upper

        // Step 2: Normalize the angles to ensure they stay within [-180, 180]
        invertedLower = NormalizeAngle(invertedLower);
        invertedUpper = NormalizeAngle(invertedUpper);

        // Return the inverted range as a new Vector2 for vectorB
        return new Vector2(invertedLower, invertedUpper);
    }

    // Helper function to normalize angles within [-180, 180]
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        return angle;
    }


    #endregion

    public void Damage(float damageAmount)
    {
        _curretHelath -= damageAmount;


        _healthBar.UpdateHealthBar(_maxHealth, _curretHelath);

        if (_curretHelath <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Tigger();
    }

    private void DetectSlope()
    {
        
        float rayDistance = 1f;
        // Cast a ray downwards from the player to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, hit.collider ? Color.green : Color.yellow);

        if (hit.collider != null)
        {
            Vector2 slopeNormal = hit.normal;

            float slopeDirection = Mathf.Sign(hit.normal.x); // Left (-1) or right (1)
            slopeAngle = -slopeDirection * Vector2.Angle(slopeNormal, Vector2.up);

        }
        /*else
        {
            slopeAngle = 0;

        }*/
    }

    private void RotatePlayer()
    {

        /*transform.rotation = Quaternion.Euler(0, 0, slopeAngle);*/
        // Smoothly rotate towards the target angle using Lerp
        Quaternion targetRotation = Quaternion.Euler(0, 0, slopeAngle / 1.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

    }
}


//4.33 0.65