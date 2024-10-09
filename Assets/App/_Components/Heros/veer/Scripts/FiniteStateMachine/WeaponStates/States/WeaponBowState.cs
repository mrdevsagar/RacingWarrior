using UnityEngine;

public class WeaponBowState : WeaponState
{
    private bool IsArrowAvailable = false;
    private bool isFiring = false;
    private float customDistance = -0.1f;

    private GameObject CurrentArrowObject;
    public WeaponBowState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, WeaponData weaponData, GameObject weaponObject) : base(player, playerStateMachine, playerData, weaponData, weaponObject)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetFloat("BlendIdle", 3);
    }

    public override void Exit()
    {
        base.Exit();
        player.Comp.Body.RightArmIKSolver.flip = true;
        RotateLeftHand(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void PhysicsLateUpdate()
    {
        base.PhysicsLateUpdate();
        BowAim();
    }

    public void BowAim()
    {
        float angle = player.input.LookInput;

        float handRotationAngle = angle;

        float rightJoysticDistance = player.input.LookDragDistance;



        if (angle.Equals(float.NaN))
        {
            player.Comp.Bow.BowTopCCDIK.weight = 0;
            player.Comp.Bow.BowBottomCCDIK.weight = 0;
            isFiring = false;
        }
        else
        {
            if (player.IsPlayerLeftFacing)
            {
                handRotationAngle += player.flipAngle;
            }
            if (rightJoysticDistance > 0.1f && rightJoysticDistance < 0.985f)
            {
                isFiring = false;
                MoveArrow(rightJoysticDistance, angle);
            } else if (rightJoysticDistance >= 0.985f)
            {
                if (!isFiring && IsArrowAvailable)
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
                    customDistance = -0.1f;  // Reset the distance for the next cycle
                    /*isFiring = false;*/  // Reset the firing state
                }
            }

            RotateLeftHand(handRotationAngle);
            player.RotateHead(angle);

        }
    }

    public void HandPull(float distance, float angle)
    {
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }
        float convertAngle = player.ConvertValue(distance, new Vector2(0.1f, 0.96f), new Vector2(0f, -0.4f));
        if (angle >= 180 && angle <= 360 && convertAngle < -0.15)
        {
            player.Comp.Body.RightArmIKSolver.flip = false;
        }
        else
        {
            player.Comp.Body.RightArmIKSolver.flip = true;
        }

        player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.localPosition = new Vector3(convertAngle, 0, 0);
    }

    private void MoveArrow(float distance, float angle)
    {
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }
        HandPull(distance, angle);
        if (distance > 0.1f && distance < 0.985f)
        {
            player.Comp.Bow.BowTopCCDIK.GetChain(0).target = player.Comp.Bow.BowFistTarget;
            player.Comp.Bow.BowBottomCCDIK.GetChain(0).target = player.Comp.Bow.BowFistTarget;

            player.Comp.Bow.BowTopCCDIK.weight = 1;
            player.Comp.Bow.BowBottomCCDIK.weight = 1;

            float v = player.ConvertValue(distance, new Vector2(0.1f, 0.985f), new Vector2(0f, -0.37f));
            player.Comp.Bow.ArrowHolder.localPosition = new Vector3(v, player.Comp.Bow.ArrowHolder.localPosition.y, player.Comp.Bow.ArrowHolder.localPosition.z);

            if (player.Comp.Bow.ArrowPrefab != null)
            {
                if (!IsArrowAvailable)
                {
                    IsArrowAvailable = true;
                    // Instantiate the prefab at the origin (0, 0, 0) with no rotation
                    CurrentArrowObject = player.InstantiateArrow(player.Comp.Bow.ArrowPrefab, Vector3.zero, player.Comp.Bow.BowGameObj.transform.rotation);
                    CurrentArrowObject.transform.parent = player.Comp.Bow._arrowPostion.transform;
                    CurrentArrowObject.transform.localPosition = Vector3.zero;
                    if (player.IsPlayerLeftFacing)
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

            if (player.IsPlayerLeftFacing)
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


        player.Comp.Bow.BowTopCCDIK.GetChain(0).target = player.Comp.Bow.BowInitialTarget;
        player.Comp.Bow.BowBottomCCDIK.GetChain(0).target = player.Comp.Bow.BowInitialTarget;
    }

    protected override void SetHandsIKSolvers()
    {
        player.Comp.Body.LeftArmIKSolver.GetChain(0).target = player.Comp.Bow.P_BOW_LeftArmTarget;
        player.Comp.Body.LeftFistIKSolver.GetChain(0).target = player.Comp.Bow.P_BOW_LeftFistTarget;

        player.Comp.Body.RightArmIKSolver.GetChain(0).target = player.Comp.Bow.P_BOW_RightArmTarget;
        player.Comp.Body.RightFistIKSolver.GetChain(0).target = player.Comp.Bow.P_BOW_RightFistTarget;
    }

    private void RotateLeftHand(float handRotationAngle)
    {
        player.Comp.Bow.P_Bow_Parent_LeftHandTarget.transform.eulerAngles = new Vector3(player.Comp.Bow.P_Bow_Parent_LeftHandTarget.transform.rotation.x, player.Comp.Bow.P_Bow_Parent_LeftHandTarget.transform.rotation.y, handRotationAngle);

        player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.eulerAngles = new Vector3(player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.rotation.x, player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.rotation.y, handRotationAngle);

    }
}

