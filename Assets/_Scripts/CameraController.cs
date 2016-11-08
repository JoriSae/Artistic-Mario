using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    #region Variables
    public Transform character;

    public Vector3 startPosition;

    [SerializeField] private float minCameraPosition, maxCameraPosition;
    [SerializeField] private float widthOffset;
    [SerializeField] private float tileOffset;
    [SerializeField] private float destructorWidth;

    private Vector3 cameraPosition;

    private BoxCollider2D objectDestructor;
    #endregion

    // Use this for initialization
    void Start ()
    {
        float halfCameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        maxCameraPosition -= halfCameraWidth - tileOffset;
        minCameraPosition += halfCameraWidth - tileOffset;

        cameraPosition = Camera.main.transform.position;

        CalculateDestructorSize( halfCameraWidth );

        transform.position = startPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if ( character.position.x > Camera.main.transform.position.x )
        {
            cameraPosition.x = character.position.x;
        }

        cameraPosition.x = Mathf.Clamp( cameraPosition.x, minCameraPosition, maxCameraPosition );
        Camera.main.transform.position = cameraPosition;
	}

    void CalculateDestructorSize ( float _halfCameraWidth )
    {
        objectDestructor = GetComponentInChildren<BoxCollider2D>();

        float xOffset = _halfCameraWidth + ( objectDestructor.size.x / 2 ) + widthOffset;

        Vector2 destructorSize = new Vector2( destructorWidth, ( Camera.main.orthographicSize * 2 ) - 0.1f );

        objectDestructor.offset = new Vector2( -xOffset, 0 );
        objectDestructor.size = destructorSize;
    }
}
