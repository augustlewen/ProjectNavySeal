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
    [System.NonSerialized] public bool isAirborne;
    [System.NonSerialized] public bool isChargingThrow;
    [System.NonSerialized] public bool isInvalidArea;

    [System.NonSerialized] public Vector3 startPosition;
    [System.NonSerialized] public Vector2 direction;

    [System.NonSerialized] public Rigidbody2D myRigidbody;

    private GameObject seal;
    public LineRenderer ropePrefab;

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

        //Draw Rope
        ropePrefab.SetPosition(0, transform.position);
        ropePrefab.SetPosition(1, new Vector2(seal.transform.position.x - 0.2f, seal.transform.position.y));
    }



    //When harpoon is in hand, it will rotate in the direction of the mouse position
    private void Rotate_Harpoon()
    {
        if (isInHand && !isChargingThrow)
        {
            transform.position = new Vector2(seal.transform.position.x + 0.05f, seal.transform.position.y - 0.13f);
            startPosition = transform.position;


            //Get the screen position of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

            direction = mouseOnScreen - positionOnScreen;
            transform.right = direction;
        }
    }

    //Change the Angle of the harpoon while airborne
    private void Airborne()
    {
        if(isAirborne)
        {
            //Get Angle of harpoon based on it's current direction
            direction = myRigidbody.velocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }



    //When the harpoon hits the wall, it sticks
    void OnCollisionEnter2D(Collision2D wall)
    {
        if (!isInHand && wall.gameObject.transform.CompareTag("Wall"))
        {
            isAirborne = false;

            //Stop Harpoon movement
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.isKinematic = true;

            hasHit = true;
        }


    }




    //An Area under the Seal, makes it impossible for the harpoon to be thrown down
    private void OnTriggerEnter2D(Collider2D area)
    {
        if (area.gameObject.transform.CompareTag("InvalidArea"))
            isInvalidArea = true;
    }

    private void OnTriggerExit2D(Collider2D area)
    {
        if (area.gameObject.transform.CompareTag("InvalidArea"))
            isInvalidArea = false;
    }
}
