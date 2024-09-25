using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UIElements;


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

   
  

    [SerializeField]
    private Vector3 v;
    
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

    [Header("New Bow Targets")]
   

    [SerializeField]
    public Transform P_BOW_LeftArmTarget;
    [SerializeField]
    public Transform P_BOW_LeftFistTarget;

    [SerializeField]
    public Transform P_BOW_RightArmTarget;
    [SerializeField]
    public Transform P_BOW_RightFistTarget;
    [Space(10)]

    public float headRotationAngle;

    public WeaponState SelectedWeapon = WeaponState.FIST;

    [Header("Weapon GameObjeets")]
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

        IdleState = new PlayerIdleState(this,StateMachine, _playerData,"idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "move");

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
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();


        if (!input.LookInput.Equals(float.NaN))
        {
            ChangeCameraPosition(true);
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

    #endregion


    #region Other Public Methods 
    public void SetVelocityX(float velocityX)
    {
        RB.velocity = new Vector2(velocityX, RB.velocity.y);
    }

    public void SetFrictionMaterial(bool isMoving)
    {
        if(isMoving)
        {
            GetComponent<Collider2D>().sharedMaterial = moveMaterial;
        }
        else
        {
            GetComponent<Collider2D>().sharedMaterial = stillMaterial;
        }

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

            Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 90 * (IsPlayerLeftFacing ? -1 : 1)) ;

            return;
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


    public void BowAim()
    {
        float angle = input.LookInput;

        float handRotationAngle = angle;

        float rightJoysticDistance = input.LookDragDistance;

        Debug.Log(angle);

        if (angle.Equals(float.NaN))
        {
            LeftArmIKSolver.GetChain(0).target = AnimLeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = AnimLeftFistTarget;

            RightArmIKSolver.GetChain(0).target = AnimRightArmTarget;
            RightFistIKSolver.GetChain(0).target = AnimRightFistTarget;

            Head.eulerAngles = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y, 90 * (IsPlayerLeftFacing ? -1 : 1));

            BowTopCCDIK.weight = 0;
            BowBottomCCDIK.weight = 0;
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

            float convertAngle = ConvertValue(rightJoysticDistance);

            if (rightJoysticDistance > 0.1f)
            {
                BowTopCCDIK.weight = 1;
                BowBottomCCDIK.weight = 1;
            } else
            {
                BowTopCCDIK.weight = 0;
                BowBottomCCDIK.weight = 0;
            }


            if (angle >= 180 && angle <= 360 && convertAngle < -0.15)
            {
                RightArmIKSolver.flip = false;
            } else
            {
                RightArmIKSolver.flip = true;
            }

            P_Bow_Parent_RightHandTarget.transform.localPosition = new Vector3(convertAngle, 0, 0);

            /* P_BOW_RightArmTarget.position = new Vector3(ConvertValue(angle), P_BOW_RightArmTarget.position.y, P_BOW_RightArmTarget.position.z);*/

            Debug.Log(angle + "angle" + convertAngle);
            /*if (rightJoysticDistance > 0.1f && rightJoysticDistance < 0.96f)
            {

               

            } else if (rightJoysticDistance >= 0.96f)
            {
               
            } else
            {
                
            }*/

            P_Bow_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(P_Bow_Parent_LeftHandTarget.transform.rotation.x, P_Bow_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

            P_Bow_Parent_RightHandTarget.transform.eulerAngles = new Vector3(P_Bow_Parent_RightHandTarget.transform.rotation.x, P_Bow_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

            RotateHead(angle);

            float ConvertValue(float value)
            {
                // Source range [0.1f, 0.96f]
                float inputMin = 0.1f;
                float inputMax = 0.96f;

                // Target range [0, -0.4f]
                float outputMin = 0f;
                float outputMax = -0.4f;

                // Linear interpolation formula
                float outputValue = outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);

                return outputValue;
            }

        }
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
        Debug.Log(angle + "  distance" + distance);
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
            
            if(SelectedWeapon == WeaponState.SORD )
            {
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(true);
            } else if (SelectedWeapon == WeaponState.AKM)
            {
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(true);
                SordGameObj.SetActive(false);
            } else if  (SelectedWeapon == WeaponState.BOW) {
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
            if ((int)SelectedWeapon >= System.Enum.GetValues(typeof(WeaponState)).Length)
            {
                SelectedWeapon = WeaponState.FIST;
                BowGameObj.SetActive(false);
                AkmGameObj.SetActive(false);
                SordGameObj.SetActive(false);
            } 



    }

    #endregion
}


//4.33 0.65