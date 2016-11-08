using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2Dimensional : MonoBehaviour
{
    #region Variables
    public enum CharacterState { smallMario, normalMario, fireMario };

    public CharacterState characterState;

    public LayerMask groundLayer;

    public GameObject projectile;

    public static int characterLives;

    public bool invulnerable = false;

    [SerializeField] private Vector3 smallMarioSize, normalMarioSize;

    [SerializeField] private float groundedCheckHeight;
    [SerializeField] private float speed;
    [SerializeField] private float friction;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceExtension;
    [SerializeField] private float jumpForceExtensionBase;
    [SerializeField] private float jumpForceExtensionDecay;
    [SerializeField] private float fireRate;

    private float time;
    private float invulnerableTime;

    private Rigidbody2D rigidBody;

    private Vector2 input;
    private Vector2 startPosition;

    private Transform selfTransform;

    private SpriteRenderer spriteRenderer;

    private float heightOffset, widthOffset;
    #endregion

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selfTransform = GetComponent<Transform>();

        startPosition = transform.position;

        //Calculate Offset
        heightOffset = spriteRenderer.bounds.extents.y;
        widthOffset = spriteRenderer.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        Movement();

        BindCharacterWithinScreenBounds();

        CharacterLogic();

        if (invulnerableTime <= 0)
        {
            invulnerable = false;
        }
        else
        {
            invulnerableTime -= Time.deltaTime;
            invulnerable = true;
        }

        if (invulnerable)
        {
            spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        }

        if (transform.position.y < 0)
        {
            Reset();
        }
    }

    void CharacterLogic()
    {
        switch (characterState)
        {
            case CharacterState.smallMario:
                selfTransform.localScale = smallMarioSize;
                break;
            case CharacterState.normalMario:
                selfTransform.localScale = normalMarioSize;
                spriteRenderer.color = Color.red;
                break;
            case CharacterState.fireMario:
                spriteRenderer.color = Color.white;

                time -= Time.deltaTime;

                if (Input.GetMouseButton(0) && time <= 0)
                {
                    FireProjectiles();
                    time = fireRate;
                }
                break;
        }
    }

    public void InvulnerableState(float time)
    {
        invulnerableTime += time;
    }

    void FireProjectiles()
    {
        GameObject fireBall = Instantiate(projectile, transform.position + new Vector3(widthOffset, 0), Quaternion.identity) as GameObject;
    }

    void UpdateInput()
    {
        //Handle Move Input
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

        //Handle Jump Input
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
        else if (input.x == -1)
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
        if (jumpForceExtension > 0)
        {
            jumpForceExtension -= jumpForceExtensionDecay * Time.deltaTime;
        }

        //Jump Extention Decay Clamping
        if (jumpForceExtension < 0)
        {
            jumpForceExtension = 0;
        }

        //Move
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        //Jump
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, (input.y == 1 && CheckGrounded()) ? jumpForce : GetComponent<Rigidbody2D>().velocity.y + jumpForceExtension);
        input.y = 0;
    }

    private void BindCharacterWithinScreenBounds()
    {
        //Bind the Character Within the Screen Bounds
        Vector2 characterPosition = transform.position;
        characterPosition.x = Mathf.Clamp(characterPosition.x, (Camera.main.transform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height) + widthOffset),
                                                               (Camera.main.transform.position.x + (Camera.main.orthographicSize * Screen.width / Screen.height) - widthOffset));
        transform.position = characterPosition;
    }

    public bool CheckGrounded()
    {
        //Check if Grounded
        ExtDebug.DrawBoxCastBox(transform.position, GetComponent<Collider2D>().bounds.extents - new Vector3(0.01f, 0.01f), Quaternion.identity, Vector2.down, groundedCheckHeight, Color.white);
        return Physics2D.BoxCast(transform.position, GetComponent<Collider2D>().bounds.extents - new Vector3(0.01f, 0.01f), 0, Vector2.down, groundedCheckHeight, groundLayer);
    }

    public void Reset()
    {
        SceneManager.LoadScene("Game");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Scene Reset"))
        {
            Reset();
        }
    }
}