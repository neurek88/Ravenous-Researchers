using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets._2D
{

    public class PlatformerCharacter2D : MonoBehaviour
    {
        private bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        public bool climbing = false;
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
        private Rigidbody2D rb;
        private float gravityStore;
        private float ladderSpeed = 1f;


        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            gravityStore = rb.gravityScale;
        }


        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
            
            //anim.SetBool("running", false);
            // Set the vertical animation
            // anim.SetFloat("vSpeed", rb.velocity.y);
        }


        public void Move(float move, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                //anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                rb.velocity = new Vector2(move*maxSpeed, rb.velocity.y);
                anim.SetBool("running", true);
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                    // ... flip the player.
                    Flip();
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                    // ... flip the player.
                    Flip();
             
            }
            // If the player should jump...
            if (grounded && jump)
            {
                // Add a vertical force to the player.
                grounded = false;
                //anim.SetBool("Ground", false);
                rb.AddForce(new Vector2(0f, jumpForce));
            }

        }
        public void Climb(float climb)
        {
            if (climbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, ladderSpeed * climb);

            }

            if (!climbing)
            {
                rb.gravityScale = gravityStore;
            }

        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}