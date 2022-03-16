using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicObject : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    protected const float MIN_MOVE_DISTANCE = 0.001f;
    protected const float CONTACT_SHELL_RADIUS = 0.010f;
    protected const float MIN_GROUND_NORMAL_Y = .65f;

    // component references
    private Rigidbody2D body;

    protected Vector2 velocity;

    private Vector2 targetVelocity;
    private Vector2 groundNormal;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        // only detect collision with objects in the same layer as this kinematic object
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            velocity.y = 3.5f;
        }
        float xAxis = Input.GetAxisRaw("Horizontal");
        targetVelocity = new Vector2(xAxis * .8f, 0f);
    }

    protected virtual void FixedUpdate()
    {
        IsGrounded = false;

        // apply gravity
        velocity += Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        Vector2 deltaPosition = velocity * Time.deltaTime;


        Vector2 groundDirection = new Vector2(groundNormal.y, -groundNormal.x) * Mathf.Sign(deltaPosition.x);
        float dot = Vector2.Dot(Vector2.up, groundDirection);
        float boost = 1.0f;
        if (dot < 0)
        {
            boost = 2.0f;
        } else if (dot > 0)
        {
            boost = .5f;
        }

        PerformHorizontalMovement(new Vector2(groundNormal.y, -groundNormal.x) * deltaPosition.x);
        //PerformMovement(Vector2.up * deltaPosition.y);

        PerformVerticalMovement(Vector2.up * deltaPosition.y);
    }

    void PerformMovement(Vector2 move)
    {
        float magnitude = move.magnitude;
        
        if (magnitude > MIN_MOVE_DISTANCE)
        {
            int hitCount = body.Cast(move, contactFilter, hitBuffer, magnitude + CONTACT_SHELL_RADIUS);
            for (int i = 0; i < hitCount; ++i)
            {
                Vector2 surfaceNormal = hitBuffer[i].normal;
                if (surfaceNormal.y > MIN_GROUND_NORMAL_Y)
                {
                    IsGrounded = true;
                }

                if (IsGrounded)
                {
                    
                    float dotProduct = Vector2.Dot(velocity, surfaceNormal);
                    if (dotProduct < 0) {
                        velocity = velocity - dotProduct * surfaceNormal;
                    }
                } else
                {
                    //velocity.x *= 0;
                    //velocity.y = Mathf.Min(velocity.y, 0);
                }
                float modifiedMagnitude = hitBuffer[i].distance - CONTACT_SHELL_RADIUS;
                magnitude = (modifiedMagnitude < magnitude) ? modifiedMagnitude : magnitude;
            }
        }
        body.position = body.position + move.normalized * magnitude;
    }

    void PerformHorizontalMovement(Vector2 move)
    {
        float magnitude = move.magnitude;
        if (magnitude > MIN_MOVE_DISTANCE)
        {
            int hitCount = body.Cast(move, contactFilter, hitBuffer, magnitude + CONTACT_SHELL_RADIUS);
            for (int i = 0; i < hitCount; ++i)
            {
                float modifiedMagnitude = hitBuffer[i].distance - CONTACT_SHELL_RADIUS;
                magnitude = (modifiedMagnitude < magnitude) ? modifiedMagnitude : magnitude;
            }
        }
        body.position = body.position + move.normalized * magnitude;
    }

    void PerformVerticalMovement(Vector2 move)
    {
        float magnitude = move.magnitude;
        if (magnitude > MIN_MOVE_DISTANCE)
        {
            int hitCount = body.Cast(move, contactFilter, hitBuffer, magnitude + CONTACT_SHELL_RADIUS);
            for (int i = 0; i < hitCount; ++i)
            {
                Vector2 surfaceNormal = hitBuffer[i].normal;

                if (surfaceNormal.y > MIN_GROUND_NORMAL_Y)
                {
                    IsGrounded = true;
                    groundNormal = surfaceNormal;
                }

                if (IsGrounded)
                {
                    velocity.y = 0;
                }

                float modifiedMagnitude = hitBuffer[i].distance - CONTACT_SHELL_RADIUS;
                magnitude = (modifiedMagnitude < magnitude) ? modifiedMagnitude : magnitude;

            }
        }
        body.position = body.position + move.normalized * magnitude;
    }
}
