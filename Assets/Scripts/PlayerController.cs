using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MyKinematicObject
{
    public float jumpTakeOffSpeed = 4.0f;

    private JumpState jumpState = JumpState.Grounded;


    protected override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && jumpState == JumpState.Grounded)
        {
            jumpState = JumpState.Prepare;
        }


        // update jump state
        switch (jumpState)
        {
            case JumpState.Prepare:
                velocity.y = jumpTakeOffSpeed;
                jumpState = JumpState.Jumping;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InAir;
                }
                break;
            case JumpState.InAir:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    velocity *= .5f;
                }
                if (IsGrounded)
                {
                    jumpState = JumpState.Grounded;
                }
                break;
            case JumpState.PrepareSpring:
                velocity.y = jumpTakeOffSpeed * 1.4f;
                jumpState = JumpState.InAirSpring;
                break;
            case JumpState.InAirSpring:
                if (IsGrounded)
                {
                    jumpState = JumpState.Grounded;
                }
                break;
        }

    }

    public void bounce(float intensity)
    {
        jumpState = JumpState.PrepareSpring;
    }

    private enum JumpState
    {
        Grounded,
        Prepare,
        PrepareSpring,
        InAirSpring,
        Jumping,
        InAir
    }
}
