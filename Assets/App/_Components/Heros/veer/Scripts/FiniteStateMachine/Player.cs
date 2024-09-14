using TMPro;
using Unity.Cinemachine;
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


    #endregion

    public GameObject RightCineMachineCamera;
    public GameObject LeftCineMachineCamera;

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

         


    }

    private void Update()
    {
        /* CurrentVelocity = RB.velocity;*/

       

        StateMachine.CurrentState.LogicUpdate();

       /* Debug.Log(input.MoveInput);*/
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion


    #region Other Functions 
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

    private void ChangeCameraPosition(bool isRightFacing)
    {
        
            RightCineMachineCamera.SetActive(isRightFacing);
            LeftCineMachineCamera.SetActive(!isRightFacing);
        
    }

    #endregion
}
