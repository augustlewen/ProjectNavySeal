using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HarpoonController : MonoBehaviour
{
    public float force;
    public float maxForce;
    private bool isInHand = true;
    private bool isChargingThrow;
    private bool hasHit;
    private Vector3 startPosition;
    private Vector2 direction;
    

    private Rigidbody2D myRigidbody;


    private void Start()
    {
        startPosition = transform.position;

        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Rotate_Harpoon();
        Throw_Harpoon();
        Airborne();
    }

    private void Rotate_Harpoon()
    {
        if (isInHand && isChargingThrow == false)
        {
            //Get the screen position of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

            direction = mouseOnScreen - positionOnScreen;
            transform.right = direction;
        }
    }

    private void Throw_Harpoon()
    {
        //if(chargingThrow == false)
        if (Input.GetButton("Throw"))
        {
            if(force < maxForce)
            {
                isChargingThrow = true;
                force += Mathf.Lerp(0f, maxForce, 0.002f);
                transform.position = Vector3.Lerp(transform.position, startPosition - transform.right, 0.002f);
            }
        }

        if (Input.GetButtonUp("Throw"))
        {
            isChargingThrow = false;
            isInHand = false;
            
            myRigidbody.AddForce(transform.right * force);
            myRigidbody.gravityScale = 0.9f;
        }
    }

    private void Airborne()
    {
        if(isInHand == false && hasHit == false)
        {
            direction = myRigidbody.velocity;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        hasHit = true;
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.isKinematic = true;
    }
}
