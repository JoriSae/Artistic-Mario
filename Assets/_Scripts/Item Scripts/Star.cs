using UnityEngine;
using System.Collections;

public class Star : Item
{
    [SerializeField] private float speed;
    [SerializeField] private float thrust;
    [SerializeField] private float invulnerableTime;
    private Rigidbody2D rigidBody;

    new void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * thrust);
    }

	void Update()
    {
        Movement();
	}

    void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    protected override void AlterPlayerState()
    {
        characterController.InvulnerableState(invulnerableTime);
    }
}
