using UnityEngine;
using System.Collections;

public class DrawEnemyColliders : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
