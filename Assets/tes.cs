using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class tes : MonoBehaviour
{
    #region Variables
    public LayerMask groundLayer;

    //public static int characterLives;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpForceExtension;
    [SerializeField]
    private float jumpForceExtensionBase;
    [SerializeField]
    private float jumpForceExtensionDecay;

    private Rigidbody2D rigidBody;

    private Vector2 input;

    private float heightOffset, widthOffset;
    #endregion

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        heightOffset = transform.GetComponent<Collider2D>().bounds.extents.y;
        widthOffset = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        BindCharacterWithinScreenBounds();

        if (input.x == 1)
        {
            speed = Mathf.Lerp(0, maxSpeed, acceleration);
        }
        else if (input.x == -1)
        {
            speed = Mathf.Lerp(0, -maxSpeed, acceleration);
        }
        else if (input.x == 0)
        {
            speed = Mathf.Lerp(maxSpeed, 0, acceleration);
        }
    }

    void UpdateInput()
    {
        //Handle input
        if (Input.GetKey(KeyCode.D))
        {
            input.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
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

            if (CheckGrounded())
            {
                jumpForceExtension = jumpForceExtensionBase;
            }
        }

        if (!Input.GetKey(KeyCode.Space))
        {
            if (!CheckGrounded())
            {
                jumpForceExtension = 0;
            }
        }
    }

    void Movement()
    {
        //Jump Extention Decay
        if (jumpForceExtension > 0)
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, (input.y == 1 && CheckGrounded()) ? jumpForce : GetComponent<Rigidbody2D>().velocity.y + jumpForceExtension);
        input.y = 0;
    }

    private void BindCharacterWithinScreenBounds()
    {
        Vector2 characterPosition = transform.position;
        characterPosition.x = Mathf.Clamp(characterPosition.x, (Camera.main.transform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height) + widthOffset),
                                                               (Camera.main.transform.position.x + (Camera.main.orthographicSize * Screen.width / Screen.height) - widthOffset));
        transform.position = characterPosition;
    }

    public bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, heightOffset + 0.1f, groundLayer);
    }
}
