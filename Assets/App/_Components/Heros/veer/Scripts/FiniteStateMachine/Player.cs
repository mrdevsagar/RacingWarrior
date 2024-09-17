using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

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

    #endregion



    /*private Vector2 _workspace;*/

    /*public Vector2 CurrentVelocity { get; private set; }*/

    /* public TextMeshProUGUI textBox;*/

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
        } else
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
