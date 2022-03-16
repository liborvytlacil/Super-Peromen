using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyKinematicObject : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    protected const float MIN_MOVE_DISTANCE = 0.001f;
    protected const float CONTACT_SHELL_RADIUS = 0.010f;

    // component references
    private Rigidbody2D body;

    protected Vector2 velocity;


    private Vector2 groundNormal;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private Vector2 targetVelocity;


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
        float xAxis = Input.GetAxisRaw("Horizontal");
        targetVelocity = new Vector2(xAxis * .8f, 0f);
    }

    protected virtual void FixedUpdate()
    {
        IsGrounded = false;

        // apply gravity
        velocity += Physics2D.gravity * Time.deltaTime;

        checkGrounded((velocity * Time.deltaTime).y * Vector2.up);

        if (IsGrounded)
        {
            velocity.y = 0f;
        }

        Vector2 direction = new Vector2(groundNormal.y, -groundNormal.x);
        PerformMovement(targetVelocity.x * Time.deltaTime * direction);

    }

    void PerformMovement(Vector2 move)
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

    private void checkGrounded(Vector2 move)
    {
        float magnitude = move.magnitude;
        if (magnitude > MIN_MOVE_DISTANCE)
        {
            int hitCount = body.Cast(move, contactFilter, hitBuffer, magnitude + CONTACT_SHELL_RADIUS);
            for (int i = 0; i < hitCount; ++i)
            {
                Vector2 surfaceNormal = hitBuffer[i].normal;
                if (surfaceNormal.y > 0)
                {
                    IsGrounded = true;
                    groundNormal = hitBuffer[i].normal;
                }
                else
                {
                    // airbone but hit something
                    velocity.y = Mathf.Min(0f, velocity.y);
                }

                float modifiedMagnitude = hitBuffer[i].distance - CONTACT_SHELL_RADIUS;
                magnitude = (modifiedMagnitude < magnitude) ? modifiedMagnitude : magnitude;
            }
        }
        body.position = body.position + move.normalized * magnitude;
    }
}
