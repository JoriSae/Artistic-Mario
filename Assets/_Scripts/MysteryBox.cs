using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MysteryBox : MonoBehaviour
{
    #region Variables
    private enum BoxState { coin, oneUp, mushroom, fireFlower, star };
    [SerializeField] private BoxState boxState;

    public LayerMask Player;

    public List<GameObject> items;
    public List<Sprite> sprites;

    public bool CharacterDied = true;

    [SerializeField] private int itemAmount = 1;

    [SerializeField] private float boxCastCheckDistance;

    private Vector2 bounds;

    private CharacterController2Dimensional characterController;
    private GameObject player;

    private bool boxActive = true;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2Dimensional>();
        bounds = GetComponent<Collider2D>().bounds.extents;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.characterState == CharacterController2Dimensional.CharacterState.normalMario && boxState == BoxState.mushroom ||
            characterController.characterState == CharacterController2Dimensional.CharacterState.fireMario && boxState == BoxState.mushroom)
        {
            //If Character is in normalMario State Change the State of MysteryBoxes
            boxState = BoxState.fireFlower;
        }
        else if (characterController.characterState == CharacterController2Dimensional.CharacterState.smallMario && boxState == BoxState.fireFlower)
        {
            //If Character is in smallMario State Change the State of MysteryBoxes
            boxState = BoxState.mushroom;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            boxState -= 1;
        }
    }

    void SpawnItem(int _itemNumber)
    {
        //Spawn Item
        GameObject spawnedItem = Instantiate(items[_itemNumber]);

        //Calculate Offset
        float yBoxOffset = GetComponent<Collider2D>().bounds.extents.y;
        float yItemOffset = items[_itemNumber].GetComponent<SpriteRenderer>().bounds.extents.y;

        //Set Position
        spawnedItem.transform.position = new Vector2(transform.position.x, transform.position.y + ((yBoxOffset + yItemOffset)));

        --itemAmount;

        //Determine if Box is Active or Inactive
        boxActive = itemAmount <= 0 ? false : true;

        if (!boxActive)
        {
            //Change Sprites
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If Character Hits the Bottom of the Mystery Box Spawn Item of Correct Type
        ExtDebug.DrawBoxCastBox(transform.position, bounds, Quaternion.identity, Vector2.down, boxCastCheckDistance, Color.white);
        if (Physics2D.BoxCast(transform.position, bounds, 0, Vector2.down, boxCastCheckDistance, Player))
        {
            if (collision.gameObject.CompareTag("Player") && boxActive)
            {
                switch (boxState)
                {
                    case BoxState.coin:
                        SpawnItem(0);
                        break;
                    case BoxState.mushroom:
                        SpawnItem(1);
                        break;
                    case BoxState.fireFlower:
                        SpawnItem(2);
                        break;
                    case BoxState.oneUp:
                        SpawnItem(3);
                        break;
                    case BoxState.star:
                        SpawnItem(4);
                        break;
                }
            }
        }
    }
}
