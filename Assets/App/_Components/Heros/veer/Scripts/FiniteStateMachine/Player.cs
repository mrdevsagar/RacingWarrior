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

    public Solver2D RightArmIKSolver;
    public Solver2D RightFistIKSolver;

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

   
    
    [Header("NewTargetsParents")]
    [SerializeField]
    private Transform P_Parent_LeftHandTarget;
    [SerializeField]
    private Transform P_Parent_RightHandTarget;
    [Space(10)]

    [Header("NewTargets")]
    [SerializeField]
    public Transform P_LeftArmTarget;
    [SerializeField]
    public Transform P_LeftFistTarget;

    [SerializeField]
    public Transform P_RightArmTarget;
    [SerializeField]
    public Transform P_RightFistTarget;
    [Space(10)]



    #endregion

    [SerializeField]
    private float flipAngle;


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

    public void RotateLeftHandStraight(float angle)
    {
        Debug.Log(angle);

        if (angle.Equals(float.NaN))
        {
            LeftArmIKSolver.GetChain(0).target = AnimLeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = AnimLeftFistTarget;

            RightArmIKSolver.GetChain(0).target = AnimRightArmTarget;
            RightFistIKSolver.GetChain(0).target = AnimRightFistTarget;
            return;
        } else
        {
            if (IsPlayerLeftFacing)
            {
                angle += flipAngle;
            }

            LeftArmIKSolver.GetChain(0).target = P_LeftArmTarget;
            LeftFistIKSolver.GetChain(0).target = P_LeftFistTarget;

            RightArmIKSolver.GetChain(0).target = P_RightArmTarget;
            RightFistIKSolver.GetChain(0).target = P_RightFistTarget;

            P_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(P_Parent_LeftHandTarget.transform.rotation.x, P_Parent_LeftHandTarget.transform.rotation.y, angle);

            P_Parent_RightHandTarget.transform.eulerAngles = new Vector3(P_Parent_RightHandTarget.transform.rotation.x, P_Parent_RightHandTarget.transform.rotation.y, angle);
        }
    }

    public void MoveBowRightHand(float angle,float distance)
    {
        Debug.Log(angle + "  distance" + distance);
    }
    #endregion

    #region Private Methods
    private void ChangeCameraPosition(bool isRightFacing)
    {
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

    #endregion
}


//4.33 0.65