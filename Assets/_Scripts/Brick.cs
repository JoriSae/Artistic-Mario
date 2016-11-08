using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    public LayerMask Player;

    protected GameObject player;
    protected CharacterController2Dimensional characterController;

    // Use this for initialization
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2Dimensional>();
    }
	
	// Update is called once per frame
	void Update()
    {

	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        ExtDebug.DrawBoxCastBox(transform.position, GetComponent<Collider2D>().bounds.extents - new Vector3(0.01f, 0.01f), Quaternion.identity, Vector2.down, .3f, Color.white);
        if (Physics2D.BoxCast(transform.position, GetComponent<Collider2D>().bounds.extents, 0, Vector2.down, .3f, Player))
        {
            if (collision.gameObject.CompareTag("Player") &&
            characterController.characterState != CharacterController2Dimensional.CharacterState.smallMario)
            {
                Destroy(gameObject);
            }
        }
    }
}
