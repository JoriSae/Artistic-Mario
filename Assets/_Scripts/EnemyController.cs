using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 right, left;

    [SerializeField] private LayerMask Player;

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
        if (collider.gameObject.CompareTag("Enemy Collider"))
        {
            moveRight = !moveRight;
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            if (Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), -Vector2.right, 0.1f, Player) ||
                Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.right, 0.1f, Player))
            {
                if (characterController.characterState != CharacterController2Dimensional.CharacterState.smallMario && characterController.invulnerable == false)
                {
                    --characterController.characterState;
                    characterController.InvulnerableState(1);
                }
                else if (characterController.invulnerable == false)
                {
                    characterController.Reset();
                }
            }
            else if (Physics2D.BoxCast(transform.position, Vector3.one, 0, Vector2.up, .3f, Player))
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
                Destroy(gameObject);
            }
        }
    }
}
