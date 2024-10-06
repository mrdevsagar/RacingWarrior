using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region State machine Reference
    public PlayerStateMachine StateMachine {  get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    #endregion

    #region Unity Variables
    public Animator Anim { get; private set; }

    [SerializeField]
    private PlayerData _playerData;

    public PlayerInputHandler input;

    public Rigidbody2D RB { get; private set; }

    public PhysicsMaterial2D moveMaterial;

    public PhysicsMaterial2D stillMaterial;


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



    public Solver2D  LeftArmIKSolver;
    public Solver2D LeftFistIKSolver;

    public LimbSolver2D RightArmIKSolver;
    public Solver2D RightFistIKSolver;

    public CCDSolver2D BowTopCCDIK;
    public CCDSolver2D BowBottomCCDIK;

    [Header("OldTargets")]
    [SerializeField]
    private Transform AnimLeftArmTarget;
    [SerializeField]
    private Transform AnimLeftFistTarget;
    [SerializeField]
    private Transform AnimRightArmTarget;
    [SerializeField]
    private Transform AnimRightFistTarget;

    [Space(10)]

    [Header("Head Bone")]
    [SerializeField]
    private Transform Head;
    [SerializeField]
    private Transform SpineBoneTransform;


    [Header("NewTargetsParents")]
    [SerializeField]
    private Transform P_AKM_Parent_LeftHandTarget;
    [SerializeField]
    private Transform P_AKM_Parent_RightHandTarget;
    [Space(10)]

    [SerializeField]
    private Transform P_Bow_Parent_LeftHandTarget;
    [SerializeField]
    private Transform P_Bow_Parent_RightHandTarget;
    [Space(10)]

    [SerializeField]
    private Transform P_Revolver_Parent_LeftHandTarget;
    [SerializeField]
    private Transform P_Revolver_Parent_RightHandTarget;
    [Space(10)]


    [Header("New AKM Targets")]
    [SerializeField]
    public Transform P_AKM_LeftArmTarget;
    [SerializeField]
    public Transform P_AKM_LeftFistTarget;

    [SerializeField]
    public Transform P_AKM_RightArmTarget;
    [SerializeField]
    public Transform P_AKM_RightFistTarget;
    [Space(10)]

    [Header("New Revolver Targets")]
    [SerializeField]
    public Transform P_Revolver_LeftArmTarget;
    [SerializeField]
    public Transform P_Revolver_LeftFistTarget;

    [SerializeField]
    public Transform P_Revolver_RightArmTarget;
    [SerializeField]
    public Transform P_Revolver_RightFistTarget;
    [Space(10)]



    [Header("New Bow Targets")]

    [SerializeField]
    private GameObject BowGameObject;

    [SerializeField]
    public Transform P_BOW_LeftArmTarget;
    [SerializeField]
    public Transform P_BOW_LeftFistTarget;

    [SerializeField]
    public Transform P_BOW_RightArmTarget;
    [SerializeField]
    public Transform P_BOW_RightFistTarget;

    [SerializeField]
    public Transform BowFistTarget;




    [SerializeField]
    public Transform BowInitialTarget;

    [Space(10)]


    [Header("Arrow")]

    [SerializeField]
    private Transform ArrowHolder;

    [SerializeField]
    private GameObject ArrowPrefab;

    private GameObject CurrentArrowObject;

    [SerializeField]
    private GameObject _arrowPostion;

    public bool IsArrowAvailable = false;

    private bool isFiring = false;
    private float customDistance = -0.5f;


    [Space(10)]


    public float headRotationAngle;

    public Weapon SelectedWeapon = Weapon.FIST;

    [Header("Weapon GameObjets")]
    [SerializeField]
    private GameObject AkmGameObj;
    [SerializeField]
    private GameObject SordGameObj;
    [SerializeField]
    private GameObject BowGameObj;

    

    #endregion


    private float flipAngle = -180;


    #region Unity Callback functions


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this,StateMachine, _playerData, "BodyIdle", "LegsIdle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "BodyWalk", "LegsWalk");

        input = GetComponent<PlayerInputHandler>();

        RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        //TODO : Initialize StateMachine

        StateMachine.Initialize(IdleState);

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
        StateMachine.CurrentState.LogicUpdate();


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
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        StateMachine.CurrentState.LatePhysicsUpdate();

    }

    private void OnJump()
    {
        StateMachine.CurrentState.OnJumpPress();
    }

    #endregion


    #region Other Public Methods 
    public void SetVelocityX(float velocityX)
    {
        Debug.LogWarning(velocityX);
        RB.velocity = new Vector2(velocityX, RB.velocity.y);
    }

    public void SetFrictionMaterial(bool isMoving)
    {
       /* if(isMoving)
        {
            GetComponent<Collider2D>().sharedMaterial = moveMaterial;
        }
        else
        {
            GetComponent<Collider2D>().sharedMaterial = stillMaterial;
        }*/

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

    public void RifleAim()
    {
        float angle = input.LookInput;

        float handRotationAngle = angle;
        
        

        if (handRotationAngle.Equals(float.NaN))
        {
            LeftArmIKSolver.GetChain(0).target = AnimLeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = AnimLeftFistTarget;

            RightArmIKSolver.GetChain(0).target = AnimRightArmTarget;
            RightFistIKSolver.GetChain(0).target = AnimRightFistTarget;

            Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 90 * (IsPlayerLeftFacing ? -1 : 1));

        } else
        {
            if (IsPlayerLeftFacing)
            {
                handRotationAngle += flipAngle;
            }
            
            LeftArmIKSolver.GetChain(0).target = P_AKM_LeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = P_AKM_LeftFistTarget;

            RightArmIKSolver.GetChain(0).target = P_AKM_RightArmTarget;
            RightFistIKSolver.GetChain(0).target = P_AKM_RightFistTarget;

            P_AKM_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(P_AKM_Parent_LeftHandTarget.transform.rotation.x, P_AKM_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

            P_AKM_Parent_RightHandTarget.transform.eulerAngles = new Vector3(P_AKM_Parent_RightHandTarget.transform.rotation.x, P_AKM_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

            RotateHead(angle);

           
        }
    }


    public void RevolverAim()
    {
        float angle = input.LookInput;

        float handRotationAngle = angle;



        if (handRotationAngle.Equals(float.NaN))
        {
            LeftArmIKSolver.GetChain(0).target = AnimLeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = AnimLeftFistTarget;

            RightArmIKSolver.GetChain(0).target = AnimRightArmTarget;
            RightFistIKSolver.GetChain(0).target = AnimRightFistTarget;

            Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 90 * (IsPlayerLeftFacing ? -1 : 1));

           
        }
        else
        {
            if (IsPlayerLeftFacing)
            {
                handRotationAngle += flipAngle;
            }

            LeftArmIKSolver.GetChain(0).target = P_Revolver_LeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = P_Revolver_LeftFistTarget;

            RightArmIKSolver.GetChain(0).target = P_Revolver_RightArmTarget;
            RightFistIKSolver.GetChain(0).target = P_Revolver_RightFistTarget;

            P_Revolver_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(P_Revolver_Parent_LeftHandTarget.transform.rotation.x, P_Revolver_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

            P_Revolver_Parent_RightHandTarget.transform.eulerAngles = new Vector3(P_Revolver_Parent_RightHandTarget.transform.rotation.x, P_Revolver_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

            RotateHead(angle);


        }
    }

    public void BowAim()
    {
        float angle = input.LookInput;

        float handRotationAngle = angle;

        float rightJoysticDistance = input.LookDragDistance;



        if (angle.Equals(float.NaN))
        {
           /* LeftArmIKSolver.GetChain(0).target = AnimLeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = AnimLeftFistTarget;

            RightArmIKSolver.GetChain(0).target = AnimRightArmTarget;
            RightFistIKSolver.GetChain(0).target = AnimRightFistTarget;
*/


            Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 90 * (IsPlayerLeftFacing ? -1 : 1));

            BowTopCCDIK.weight = 0;
            BowBottomCCDIK.weight = 0;
            isFiring = false;
        }
        else
        {
            if (IsPlayerLeftFacing)
            {
                handRotationAngle += flipAngle;
            }

            LeftArmIKSolver.GetChain(0).target = P_BOW_LeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = P_BOW_LeftFistTarget;

            RightArmIKSolver.GetChain(0).target = P_BOW_RightArmTarget;
            RightFistIKSolver.GetChain(0).target = P_BOW_RightFistTarget;

            
            /*            ConvertValue(rightJoysticDistance);*/

            if (rightJoysticDistance > 0.1f && rightJoysticDistance < 0.985f)
            {
                isFiring = false;
                MoveArrow(rightJoysticDistance, angle);
            } else if (rightJoysticDistance >= 0.985f)
            {
                if (!isFiring&& IsArrowAvailable)
                {
                    FireArrow(rightJoysticDistance, angle);
                    isFiring = true;
                }
            }

            if (isFiring)
            {
                customDistance += Time.deltaTime;  // Increase the custom distance over time
              
                MoveArrow(customDistance,angle);  // Move the arrow with custom distance

                if (customDistance >= 0.985f)
                {
                    FireArrow(customDistance, angle);
                    customDistance = -0.5f;  // Reset the distance for the next cycle
                    /*isFiring = false;*/  // Reset the firing state
                }
            }


            



            P_Bow_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(P_Bow_Parent_LeftHandTarget.transform.rotation.x, P_Bow_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

            P_Bow_Parent_RightHandTarget.transform.eulerAngles = new Vector3(P_Bow_Parent_RightHandTarget.transform.rotation.x, P_Bow_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

            RotateHead(angle);

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
        StateMachine.CurrentState.SetCurrentBodyAnimation(false);
        Anim.SetBool("SordAttck", true);
    }

    public void HandPull(float distance, float angle)
    {
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }
        float convertAngle = ConvertValue(distance, new Vector2(0.1f, 0.96f), new Vector2(0f, -0.4f));
        if (angle >= 180 && angle <= 360 && convertAngle < -0.15)
        {
            RightArmIKSolver.flip = false;
        }
        else
        {
            RightArmIKSolver.flip = true;
        }

        P_Bow_Parent_RightHandTarget.transform.localPosition = new Vector3(convertAngle, 0, 0);
    }

    private void MoveArrow(float distance, float angle)
    {
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }
        HandPull(distance,angle);
        if (distance > 0.1f && distance < 0.985f)
        {
            BowTopCCDIK.GetChain(0).target = BowFistTarget;
            BowBottomCCDIK.GetChain(0).target = BowFistTarget;

            BowTopCCDIK.weight = 1;
            BowBottomCCDIK.weight = 1;

            float v = ConvertValue(distance, new Vector2(0.1f, 0.985f), new Vector2(0f, -0.37f));
            ArrowHolder.localPosition = new Vector3(v, ArrowHolder.localPosition.y, ArrowHolder.localPosition.z);

            if (ArrowPrefab != null)
            {
                if (!IsArrowAvailable)
                {
                    IsArrowAvailable = true;
                    // Instantiate the prefab at the origin (0, 0, 0) with no rotation
                    CurrentArrowObject = Instantiate(ArrowPrefab, Vector3.zero, BowGameObject.transform.rotation);
                    CurrentArrowObject.transform.parent = _arrowPostion.transform;
                    CurrentArrowObject.transform.localPosition = Vector3.zero;
                    if (IsPlayerLeftFacing)
                    {
                        CurrentArrowObject.transform.localScale = new Vector3(-1 * CurrentArrowObject.transform.localScale.x, CurrentArrowObject.transform.localScale.y, CurrentArrowObject.transform.localScale.z);
                    }

                }

                // Optional: Set the parent of the new object to this script's GameObject

            }
            else
            {
                IsArrowAvailable = false;
                Debug.LogError("Prefab not assigned in the inspector.");
            }

        }
        
    }

    private void FireArrow(float distance, float angle)
    {
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }

        HandPull(distance, angle);
        Debug.Log("fired arrow");
        if (CurrentArrowObject != null)
        {
            CurrentArrowObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (IsPlayerLeftFacing)
            {
                CurrentArrowObject.GetComponent<Rigidbody2D>().AddForce(-CurrentArrowObject.transform.right * 20f, ForceMode2D.Impulse);
            }
            else
            {
                CurrentArrowObject.GetComponent<Rigidbody2D>().AddForce(CurrentArrowObject.transform.right * 20f, ForceMode2D.Impulse);
            }
        

            CurrentArrowObject.transform.parent = null;
        }
        CurrentArrowObject = null;
            IsArrowAvailable = false;


        BowTopCCDIK.GetChain(0).target = BowInitialTarget;
        BowBottomCCDIK.GetChain(0).target = BowInitialTarget;
    }

    public float ConvertValue(float value, Vector2 inputRange, Vector2 outputRange)
    {
        // Linear interpolation formula
        float outputValue = outputRange.x + (value - inputRange.x) * (outputRange.y - outputRange.x) / (inputRange.y - inputRange.x);

        return outputValue;
    }
    private void RotateHead(float angle)
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

       

        Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, headRotationAngle);

        
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
        StateMachine.CurrentState.SetCurrentBodyAnimation(true);
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

        AttackSword();
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

            // Increment the weapon state
            SelectedWeapon++;
            
            if(SelectedWeapon == Weapon.SWORD )
            {
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(true);
            } else if (SelectedWeapon == Weapon.AKM)
            {
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(true);
                SordGameObj.SetActive(false);
            } else if  (SelectedWeapon == Weapon.BOW) {
                BowGameObj.SetActive(true);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(false);
            } else
            {
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(false);
            }

            // If we've gone past the last weapon state, loop back to the first one
            if ((int)SelectedWeapon >= System.Enum.GetValues(typeof(Weapon)).Length)
            {
                SelectedWeapon = Weapon.FIST;
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(false);
            } 



    }

    #endregion
}


//4.33 0.65