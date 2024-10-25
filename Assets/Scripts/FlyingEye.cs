using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float wayPointReachedDistance = 0.1f;
    public DetectionZone attack1DetectionZone;
    public List<Transform> wayPoints;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWayPoint;
    int wayPointNum;

    public bool _hasTarget = false;
    

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>(); 
    }

    private void Start()
    {
        nextWayPoint = wayPoints[wayPointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attack1DetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            // Dead flying eye falls to the ground
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Flight()
    {
        // Fly to the next way point
        Vector2 directionToWwayPoint = (nextWayPoint.position - transform.position).normalized;

        // Check if we have reached the way point already
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        rb.velocity = directionToWwayPoint * flightSpeed;
        FlipDirection();

        // See if we swtiched way points
        if (distance <= wayPointReachedDistance)
        {
            // Switch to next way point
            wayPointNum++;

            if (wayPointNum >= wayPoints.Count)
            {
                // Loop back to original way point
                wayPointNum = 0;
            }

            nextWayPoint = wayPoints[wayPointNum];
        }
    }

    private void FlipDirection()
    {
        Vector3 localScale = transform.localScale;

        if (transform.localScale.x > 0)
        {
            // Face to right
            if (rb.velocity.x < 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }
        }
        else
        {
            // Face to left
            if (rb.velocity.x > 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }             
        }
    }
}
