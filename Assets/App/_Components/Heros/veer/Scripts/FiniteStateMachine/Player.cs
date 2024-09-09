using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine {  get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
       
    public Animator Anim { get; private set; }

    [SerializeField]
    private PlayerData _playerData;

    public PlayerInputHandler input;

    public Rigidbody2D RB { get; private set; }

    /*private Vector2 _workspace;*/

    /*public Vector2 CurrentVelocity { get; private set; }*/

   /* public TextMeshProUGUI textBox;*/

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

        /*Debug.Log(input.MoveInput);*/
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocityX)
    {
        RB.velocity = new Vector2(velocityX, RB.velocity.y);
    }

}
