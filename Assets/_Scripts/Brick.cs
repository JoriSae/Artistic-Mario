using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    public LayerMask Player;

    protected GameObject player;
    protected CharacterController2Dimensional characterController;

    private Vector2 bounds;
    [SerializeField] private float boxCastCheckDistance;

    // Use this for initialization
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2Dimensional>();

        bounds = GetComponent<Collider2D>().bounds.extents;
    }
	
	// Update is called once per frame
	void Update()
    {

	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if player collided with box and if player is not in small mario state destroy box
        ExtDebug.DrawBoxCastBox(transform.position, bounds, Quaternion.identity, Vector2.down, boxCastCheckDistance, Color.white);
        if (Physics2D.BoxCast(transform.position, bounds, 0, Vector2.down, boxCastCheckDistance, Player))
        {
            if (collision.gameObject.CompareTag("Player") &&
            characterController.characterState != CharacterController2Dimensional.CharacterState.smallMario)
            {
                Destroy(gameObject);
            }
        }
    }
}
