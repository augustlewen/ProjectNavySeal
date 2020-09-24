using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HarpoonController : MonoBehaviour
{
    [System.NonSerialized] public bool chargeHarpoon;
    [System.NonSerialized] public bool throwHarpoon;
    [System.NonSerialized] public bool recoverHarpoon;


    //Variables
    public float force;
    public float maxForce;
    public float chargeTime;
    [System.NonSerialized] public bool hasHit;
    [System.NonSerialized] public bool isInHand = true;
    [System.NonSerialized] public bool isAirborne;
    [System.NonSerialized] public bool isChargingThrow;
    [System.NonSerialized] public bool isInvalidArea;

    [System.NonSerialized] public Vector3 startPosition;
    [System.NonSerialized] public Vector2 direction;

    [System.NonSerialized] public Rigidbody2D myRigidbody;

    private GameObject seal;
    private SealMovement sealScript;
    public LineRenderer ropePrefab;

    private void Start()
    {
        startPosition = transform.position;

        myRigidbody = GetComponent<Rigidbody2D>();
        seal = GameObject.FindGameObjectWithTag("Seal");
        sealScript = seal.GetComponent<SealMovement>();
    }

    private void Update()
    {
        Rotate_Harpoon();
        Charge_Harpoon();
        Throw_Harpoon();
        Recover_Harpoon();

        Airborne();

        //Draw Rope
        ropePrefab.SetPosition(0, transform.position);
        ropePrefab.SetPosition(1, new Vector2(seal.transform.position.x - 0.1f, seal.transform.position.y));
    }


    //Hold down Throw button to build up force
    private void Charge_Harpoon()
    {
        if (chargeHarpoon)
        {

            if (isInHand && !isInvalidArea && sealScript.isGrounded)
            {
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);

                if (force < maxForce)
                {
                    isChargingThrow = true;

                    //Builds up Force over time
                    force += Mathf.Lerp(0f, maxForce, chargeTime * Time.deltaTime);

                    transform.position = Vector3.Lerp(transform.localPosition, startPosition - transform.right / 2.5f, chargeTime * Time.deltaTime);
                }

            }
        }

    }

    //Release Throw button to throw the harpoon with the force that you've built up
    private void Throw_Harpoon()
    {
        if (throwHarpoon)
        {

            if (isInHand && !isInvalidArea && sealScript.isGrounded)
            {

                isChargingThrow = false;
                isInHand = false;
                isAirborne = true;

                //Calculate Force
                myRigidbody.AddForce(transform.right * force);
                myRigidbody.gravityScale = 0.6f;
            }
        }

    }

    private void Recover_Harpoon()
    {
        if(recoverHarpoon)
        {

            if (!isInHand)
            {
                isInHand = true;
                sealScript.squish = false;

                startPosition = transform.position;

                force = 0f;
                ropePrefab.enabled = true;
                myRigidbody.isKinematic = false;
                myRigidbody.gravityScale = 0f;

            }
        }
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
