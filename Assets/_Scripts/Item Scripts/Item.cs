using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    protected GameObject player;
    protected CharacterController2Dimensional characterController;

	// Use this for initialization
	protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2Dimensional>();
	}
	
    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AlterPlayerState();
            Destroy(gameObject);
        }
    }

    protected virtual void AlterPlayerState()
    {

    }
}
