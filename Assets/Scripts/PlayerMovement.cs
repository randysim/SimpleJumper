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
        float groundedMultiplier = grounded ? 1f : 0.8f;
        if (moveLeft)
        {
            PlayerBody.velocity = new Vector2(-10 * PlayerMovementSpeed * Time.deltaTime * groundedMultiplier, PlayerBody.velocity.y);
        }
        if (moveRight)
        {
            PlayerBody.velocity = new Vector2(10 * PlayerMovementSpeed * Time.deltaTime * groundedMultiplier, PlayerBody.velocity.y);
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

        if ((!moveLeft && !moveRight) || (moveLeft && moveRight))
        {
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }

        wallDetector();
    }

    private bool isGrounded ()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();

        /* DRAW MULTIPLE RAYS FOR EDGE DETECTION (won't jump if on edge of platform)*/
        RaycastHit2D raycast = Physics2D.Raycast(
                 playerCollider.bounds.center,
                 Vector2.down,
                 playerCollider.bounds.extents.y + 0.1f,
                 platform // layermask to prevent the ray hitting player bug
            );

        RaycastHit2D leftRaycast = Physics2D.Raycast(
                 new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.center.y),
                 Vector2.down,
                 playerCollider.bounds.extents.y + 0.1f,
                 platform // layermask to prevent the ray hitting player bug
            );
        RaycastHit2D rightRaycast = Physics2D.Raycast(
                 new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.center.y),
                 Vector2.down,
                 playerCollider.bounds.extents.y + 0.1f,
                 platform // layermask to prevent the ray hitting player bug
            ); 

        return raycast.collider != null || leftRaycast.collider != null || rightRaycast.collider != null;
    }

    private void wallDetector ()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();
       if (PlayerBody.velocity.x < 0)
        {
            // left wall
            RaycastHit2D leftRaycast = Physics2D.Raycast(
                 playerCollider.bounds.center,
                 Vector2.left,
                 playerCollider.bounds.extents.x + 0.1f,
                 platform // layermask to prevent the ray hitting player bug
            );

            if (leftRaycast.collider != null)
            {
                PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
            }

        } else if (PlayerBody.velocity.x > 0)
        {
            // right wall
            RaycastHit2D rightRaycast = Physics2D.Raycast(
                 playerCollider.bounds.center,
                 Vector2.right,
                 playerCollider.bounds.extents.x + 0.1f,
                 platform // layermask to prevent the ray hitting player bug
            );

            if (rightRaycast.collider != null)
            {
                PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
            }
        }
    }

    public object this[string propertyName]
    {
        get
        {
            // probably faster without reflection:
            // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
            // instead of the following
            System.Type myType = typeof(PlayerMovement);
            System.Reflection.FieldInfo myPropInfo = myType.GetField(propertyName);
            return myPropInfo.GetValue(this);
        }
        set
        {
            System.Type myType = typeof(PlayerMovement);
            System.Reflection.FieldInfo myPropInfo = myType.GetField(propertyName);
            myPropInfo.SetValue(this, value);
        }
    }
}
