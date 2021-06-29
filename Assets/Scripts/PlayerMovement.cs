using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* PUBLIC */
    public float PlayerJumpForce;
    public float PlayerMovementSpeed;
    public Rigidbody2D PlayerBody;
    [SerializeField] private LayerMask platform;

    /* PRIVATE */
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool jump = false;
    private bool grounded = true;

    private void Update()
    {
        moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
            // set it to false in FixedUpdate to prevent jump from not registering
        }
        

        
    }

    private void FixedUpdate()
    {

        grounded = isGrounded();
        float groundedMultiplier = grounded ? 1f : 0.5f;
        if (moveLeft)
        {
            
            PlayerBody.AddForce(
                new Vector2(-250 * PlayerMovementSpeed * Time.deltaTime * groundedMultiplier, 0),
                ForceMode2D.Force
            );
        }
        if (moveRight)
        {
            PlayerBody.AddForce(
                new Vector2(250 * PlayerMovementSpeed * Time.deltaTime * groundedMultiplier, 0),
                ForceMode2D.Force
            );
        }  

        if (jump)
        {
            jump = false;
            if (grounded)
            {
                Debug.Log("JUMP");
                PlayerBody.AddForce(
                    new Vector2(0, 50 * PlayerJumpForce * Time.deltaTime),
                    ForceMode2D.Impulse
                );
            }
        }

        if (!moveLeft && !moveRight)
        {
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
    }

    private bool isGrounded ()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();

        /* DRAW MULTIPLE RAYS FOR EDGE DETECTION (won't jump if on edge of platform)*/
        RaycastHit2D raycast = Physics2D.Raycast(
                 playerCollider.bounds.center,
                 Vector2.down,
                 playerCollider.bounds.extents.y + 0.01f,
                 platform // layermask to prevent the ray hitting player bug
            );
        RaycastHit2D leftRaycast = Physics2D.Raycast(
                 new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.center.y),
                 Vector2.down,
                 playerCollider.bounds.extents.y - 0.01f,
                 platform // layermask to prevent the ray hitting player bug
            );
        RaycastHit2D rightRaycast = Physics2D.Raycast(
                 new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.center.y),
                 Vector2.down,
                 playerCollider.bounds.extents.y - 0.01f,
                 platform // layermask to prevent the ray hitting player bug
            ); 

        return raycast.collider != null || leftRaycast.collider != null || rightRaycast.collider != null;
    }
}
