using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HarpoonController : MonoBehaviour
{
    public float force;
    public float maxForce;

    [System.NonSerialized] public bool hasHit;
    [System.NonSerialized] public bool isInHand = true;
    [System.NonSerialized] public bool isChargingThrow;
    
    [System.NonSerialized] public Vector3 startPosition;
    [System.NonSerialized] public Vector2 direction;

    [System.NonSerialized] public Rigidbody2D myRigidbody;

    private GameObject seal;
    public LineRenderer linePrefab;

    private void Start()
    {
        startPosition = transform.position;

        myRigidbody = GetComponent<Rigidbody2D>();
        seal = GameObject.FindGameObjectWithTag("Seal");
    }

    private void Update()
    {
        Rotate_Harpoon();
        Airborne();
    }

    private void Rotate_Harpoon()
    {
        if (isInHand && isChargingThrow == false)
        {

            transform.position = new Vector2(seal.transform.position.x + 0.05f, seal.transform.position.y - 0.13f);

            //Get the screen position of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

            direction = mouseOnScreen - positionOnScreen;
            transform.right = direction;
        }
    }

    private void Airborne()
    {
        if(isInHand == false && hasHit == false)
        {
            //Get Angle of harpoon based on it's current direction
            direction = myRigidbody.velocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Draw Line
            linePrefab.enabled = true;
            linePrefab.SetPosition(0, transform.position);
            linePrefab.SetPosition(1, new Vector2(seal.transform.position.x - 0.2f, seal.transform.position.y) );
        }
    }

    void OnCollisionEnter2D(Collision2D wall)
    {
        if(isInHand == false && wall.gameObject.transform.CompareTag("Wall"))
        {
            hasHit = true;

            //Stop Harpoon movement
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.isKinematic = true;
        }
    }
}
