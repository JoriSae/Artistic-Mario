using UnityEngine;
using System.Collections;

public class FireFlower : Item
{
	// Update is called once per frame
	void Update ()
    {
	
	}

    protected override void AlterPlayerState()
    {
        characterController.characterState = CharacterController2Dimensional.CharacterState.fireMario;
    }
}
