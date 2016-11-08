using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Destroy Anything that Collides with Self
        Destroy(collider.gameObject);
    }
}
