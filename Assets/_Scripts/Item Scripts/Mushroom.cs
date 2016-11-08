using UnityEngine;
using System.Collections;

public class Mushroom : Item
{
    [SerializeField] private float speed;

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
        characterController.characterState = CharacterController2Dimensional.CharacterState.normalMario;
    }
}
