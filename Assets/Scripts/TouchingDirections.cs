using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses the collider to check directions to see if the object is currently on the ground, touching the wall or touching the ceiling
public class TouchingDirections : MonoBehaviour
{
    [SerializeField] 
    ContactFilter2D contactFilter;

    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    private bool _isGrounded;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private bool _isTouchingWall;
    public bool IsTouchingWall
    {
        get
        {
            return _isTouchingWall;
        }
        private set
        {
            _isTouchingWall = value;
            animator.SetBool(AnimationStrings.isTouchingWall, value);
        }
    }
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private bool _isTouchingCeiling;
    public bool IsTouchingCeiling
    {
        get
        {
            return _isTouchingCeiling;
        }
        private set
        {
            _isTouchingCeiling = value;
            animator.SetBool(AnimationStrings.isTouchingCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
        IsTouchingWall = touchingCol.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        IsTouchingCeiling = touchingCol.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDistance) > 0;
    }
}
