using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    bool facingRight = true;   //Direction Facing

    [SerializeField] float maxSpeed = 10f;     // Fastest X travel
    [SerializeField] float jumpForce = 400f;     // Amount of force for jumping

    [SerializeField] bool airControl = false;    // Can player movein air
    [SerializeField] LayerMask WhatIsGround;     // detects ground

    Transform groundCheck;                        // Position to mark where to check for ground
    float groundedRadius = .2f;
    bool grounded = false;
    Transform ceilingCheck;
    float ceilingRadius = .01f;
    Animator anim;
    Rigidbody2D rb;

    Transform playerGraphics;

    // Use this for initialization
    void Awake () {
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerGraphics = transform.Find("Graphics");
        if (playerGraphics == null)
        {
            Debug.Log("No player graphics");
        }
	}
	
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, WhatIsGround);
        anim.SetBool("running", grounded);

    }

    public void Move(float move, bool jump)
    {
        if (grounded || airControl)
        {
            anim.SetFloat("Speed", Mathf.Abs(move));

            rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move <0 && facingRight)
            {
                Flip();
            }
        }

        if (grounded && jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
    void Flip ()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
