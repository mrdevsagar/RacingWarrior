using System;
using UnityEngine;

public class WeaponBowState : WeaponState
{
    private bool IsArrowAvailable = false;
    private bool isFiring = false;
    private float customDistance = 0.1f;

    private bool _isFirstArrow = true;

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

        player.Comp.Bow.P_BOW_LeftArmTarget.transform.parent = player.Comp.Bow.P_Bow_Parent_LeftHandTarget;
        player.Comp.Bow.P_BOW_LeftFistTarget.transform.parent = player.Comp.Bow.P_Bow_Parent_LeftHandTarget;

        player.Comp.Bow.P_Bow_Parent_LeftHandTarget.transform.SetLocalPositionAndRotation(weaponData.ParentLeftHandTargetBow.position,Quaternion.identity);

        player.Comp.Bow.P_BOW_LeftArmTarget.transform.localPosition = weaponData.LeftArmBow.position ;
        player.Comp.Bow.P_BOW_LeftFistTarget.transform.localPosition = weaponData.LeftFistBow.position;


        player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.parent = player.Comp.Bow.BowInitialTarget.transform.parent;


        
        player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.localPosition = weaponData.ParentRightHandTargetBow.position;

        player.Comp.Bow.P_Bow_Parent_RightHandTarget.transform.localScale = new Vector3(1,1,1);
        player.Comp.Bow.P_BOW_RightFistTarget.transform.localPosition = Vector3.zero;


        player.Comp.Bow.P_BOW_RightArmTarget.transform.localPosition = weaponData.RightArmBow.position;
        player.Comp.Bow.P_BOW_RightFistTarget.transform.localPosition = weaponData.RightFistBow.position;


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
        BowAim(player.input.LookInput, player.input.LookDragDistance, player.input.LookDragDistance >= 0.985f);
/*        if (player.input.LookDragDistance <= 0.985f)
        {
            _isFirstArrow = true;
        }*/
        /*BowAim(Angle, Distance, IsFiring);*/
    }

    public void BowAim(float angle,float distance,bool isFiring)
    {
        /*float angle = player.input.LookInput;*/

        float handRotationAngle = angle;

        if (!player.input.IsTouching)
        {
            _isFirstArrow = true;
        }

        if (angle.Equals(float.NaN))
        {
            player.Comp.Bow.BowTopCCDIK.weight = 0;
            player.Comp.Bow.BowBottomCCDIK.weight = 0;
            isFiring = false;
            _isFirstArrow = true;
        }
        else
        {
            if (player.IsPlayerLeftFacing)
            {
                handRotationAngle += player.flipAngle;
            }
            if (distance > 0.1f && distance < 0.985f && player.input.IsTouching)
            {
                isFiring = false;
                _isFirstArrow = true;
                MoveArrow(distance, angle);

            }
            else
            if (isFiring && _isFirstArrow)
            {
                _isFirstArrow = false;
                 FireArrow(distance, angle);
                HandPull(0, angle);
            }

            if (isFiring && player.input.IsTouching)
            {
                if (_isFirstArrow)
                {
                    customDistance += 0.986f;
                } 
                 
                customDistance += Time.deltaTime;
                
                _isFirstArrow = false;
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
      
        if (CurrentArrowObject != null)
        {
            
            CurrentArrowObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (player.IsPlayerLeftFacing)
            {
                CurrentArrowObject.GetComponent<Rigidbody2D>().AddForce(-CurrentArrowObject.transform.right * weaponData.ArrowSpeed, ForceMode2D.Impulse);
                CurrentArrowObject.GetComponent<ArrowStick>().IsFiredLeft = true;
            }
            else
            {
                CurrentArrowObject.GetComponent<Rigidbody2D>().AddForce(CurrentArrowObject.transform.right * weaponData.ArrowSpeed, ForceMode2D.Impulse);
                CurrentArrowObject.GetComponent<ArrowStick>().IsFiredRight = true;
            }
            CurrentArrowObject.GetComponent<BoxCollider2D>().enabled = true;
           
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

    protected override void HandleFiringChanged(bool isFiring)
    {
        base.HandleFiringChanged(isFiring);
       /* FireArrow(Distance, Angle);*/

        if (!isFiring)
        {
            _isFirstArrow = true;
        }
    }

   /* protected override void HandleDistanceChanged(float distance)
    {
        base.HandleDistanceChanged(distance);
        MoveArrow(distance, Angle);
    }

    protected override void HandleAngleChanged(float angle)
    {
        base.HandleAngleChanged(angle);
        BowAim(angle);
    }*/
}

