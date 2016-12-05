using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 right, left;

    [SerializeField] private LayerMask Player;

    [SerializeField] private Vector2 bounds;
    [SerializeField] private Vector2 playerJumpHeight;
    [SerializeField] private float boxCastCheckDistance;
    [SerializeField] private float rayCastCheckDistance;
    [SerializeField] private float invinceTime;

    private bool moveRight = true;
    
    private GameObject player;
    private CharacterController2Dimensional characterController;

    // Use this for initialization
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2Dimensional>();
    }

	// Update is called once per frame
	void Update()
    {
        //Update direction
	    if (moveRight)
        {
            movement(right);
        }
        else
        {
            movement(left);
        }
	}

    void movement(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Change direction upon collision with enemy collider
        if (collider.gameObject.CompareTag("Enemy Collider"))
        {
            moveRight = !moveRight;
        }

        //Check if the player is above, if so destroy self and add upwards velocity to player
        if (Physics2D.BoxCast(transform.position, bounds, 0, Vector2.up, boxCastCheckDistance, Player))
        {
            player.GetComponent<Rigidbody2D>().velocity = playerJumpHeight;
            Destroy(gameObject);
        }
        //Check if player collided with enemy on the left or right side and damage player accordingly
        else if (Physics2D.Raycast(new Vector2(transform.position.x - bounds.x, transform.position.y), -Vector2.right, rayCastCheckDistance, Player) ||
                 Physics2D.Raycast(new Vector2(transform.position.x + bounds.x, transform.position.y), Vector2.right, rayCastCheckDistance, Player))
        {
            if (characterController.characterState != CharacterController2Dimensional.CharacterState.smallMario && characterController.invulnerable == false)
            {
                --characterController.characterState;
                characterController.InvulnerableState(invinceTime);
            }
            else if (characterController.invulnerable == false)
            {
                characterController.Reset();
            }
        }   
    }
}
