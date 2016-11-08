using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destructionTime;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, destructionTime);
	}
	
	// Update is called once per frame
	void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
	}
}