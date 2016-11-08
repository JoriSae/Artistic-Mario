/********************************************************
 * 2D Meatboy style controller written entirely by Nyero.
 *
 * Thank you for using this script, it makes me feel all
 * warm and happy inside. ;)
 *                             -Nyero
 *
 * ------------------------------------------------------
 * Notes on usage:
 *     Please don't use the meatboy image, as your some
 * might consider it stealing.  Simply replace the sprite
 * used, and you'll have a 2D platform controller that is
 * very similar to meatboy.
 ********************************************************/
using UnityEngine;
using System.Collections;

public class CharacterController_2D : MonoBehaviour
{
    //Feel free to tweak these values in the inspector to perfection.  I prefer them private.
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float friction;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpForceExtension;
    [SerializeField] float jumpForceExtensionBase;
    [SerializeField] float jumpForceExtensionDecay;

    private Vector2 input;

    private CC2D_GroundState groundState;

    void Start()
    {
        //Create an object to check if player is grounded or touching wall
        groundState = new CC2D_GroundState(transform.gameObject);
    }

    void Update()
    {
        UpdateInput();
        UpdateMovement();
    }

    void UpdateInput()
    {
        //Handle input
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input.x = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            input.x = -1;
        }
        else
        {
            input.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            input.y = 1;

            if(groundState.isGround())
            {
                jumpForceExtension = jumpForceExtensionBase;
            }
        }

        if(!Input.GetKey(KeyCode.Space))
        {
            if(!groundState.isGround())
            {
                jumpForceExtension = 0;
            }
        }
            

    }

    void UpdateMovement()
    {
        //Friction
        if (speed >= friction)
        {
            speed -= friction;
        }
        else if (speed <= -friction)
        {
            speed += friction;
        }

        //Acceleration
        if (input.x == 1)
        {
            speed += acceleration;
        }
        else if(input.x == -1)
        {
            speed -= acceleration;
        }

        //Speed Snapping
        if (speed < friction && speed > -friction)
        {
            speed = 0;
        }

        //Speed Clamping
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        //Jump Extention Decay
        if(jumpForceExtension > 0)
        {
            jumpForceExtension -= jumpForceExtensionDecay * Time.deltaTime;
        }

        //Jump Extention Decay Clamping
        if (jumpForceExtension < 0)
        {
            jumpForceExtension = 0;
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, (input.y == 1 && groundState.isGround()) ? jumpForce : GetComponent<Rigidbody2D>().velocity.y + jumpForceExtension);
        input.y = 0;
    }
}

public class CC2D_GroundState
{
    public GameObject player;
    private float width;
    private float height;
    private float length;

    //GroundState constructor.  Sets offsets for raycasting.
    public CC2D_GroundState(GameObject playerRef)
    {
        player = playerRef;
        width = player.GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = player.GetComponent<Collider2D>().bounds.extents.y + 0.2f;
        length = 0.05f;
    }

    //Returns whether or not player is touching wall.
    public bool isWall()
    {
        bool left = Physics2D.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length);
        bool right = Physics2D.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length);

        if (left || right)
            return true;
        else
            return false;
    }

    //Returns whether or not player is touching ground.
    public bool isGround()
    {
        bool bottom1 = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y - height), -Vector2.up, length);
        bool bottom2 = Physics2D.Raycast(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length);
        bool bottom3 = Physics2D.Raycast(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length);

        if (bottom1 || bottom2 || bottom3)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //Returns whether or not player is touching wall or ground.
    public bool isTouching()
    {
        if (isGround() || isWall())
            return true;
        else
            return false;
    }

    //Returns direction of wall.
    public int wallDirection()
    {
        bool left = Physics2D.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length);
        bool right = Physics2D.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length);

        if (left)
            return -1;
        else if (right)
            return 1;
        else
            return 0;
    }
}
