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
    private Vector2 positionOnScreen;
    private Vector2 mouseOnScreen;

    [System.NonSerialized] public Rigidbody2D myRigidbody;
    private GameObject seal;
    private SealMovement sealScript;
    public GameObject iceParticlePrefab;
    private AudioSource impactSound; 

    

    private void Start()
    {
        startPosition = transform.position;

        myRigidbody = GetComponent<Rigidbody2D>();
        seal = GameObject.FindGameObjectWithTag("Seal");
        sealScript = seal.GetComponent<SealMovement>();
        impactSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Rotate_Harpoon();
        Charge_Harpoon();
        Throw_Harpoon();
        Recover_Harpoon();

        Airborne();


        //Get Mouse position
        positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

       
    }


    //Hold down Throw button to build up force
    private void Charge_Harpoon()
    {
        if (chargeHarpoon)
        {
            sealScript.myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);

            if (isInHand && !isInvalidArea && sealScript.isGrounded)
            {
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
                isAirborne = true;
                isChargingThrow = false;
                isInHand = false;

                //Calculate Force
                myRigidbody.AddForce(transform.right * force);
                myRigidbody.gravityScale = 0.6f;

            }
        }

    }

    //Get the Harpoon to return to you after it's been thrown
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
                myRigidbody.isKinematic = false;
                myRigidbody.gravityScale = 0f;
            }
        }
    }



    //Change the Angle of the harpoon while airborne
    private void Airborne()
    {
        if(isAirborne)
        {
            if(myRigidbody.velocity.magnitude > 0.0001f)
            {
                //Get Angle of harpoon based on it's current direction
                direction = myRigidbody.velocity;
            }
            else //This is to fix the bug where the harpoon gets stuck in the wall at the very first frame of throwing
            {
                direction = (mouseOnScreen - positionOnScreen).normalized;
            }

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
            impactSound.Play(0);

            GameObject ice = Instantiate(iceParticlePrefab, wall.contacts[0].point, Quaternion.Euler(direction));
            Destroy(ice, 1f);

            //Stop Harpoon movement
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.isKinematic = true;

            hasHit = true;
        }

    }




    //When harpoon is in hand, it will rotate in the direction of the mouse position
    private void Rotate_Harpoon()
    {
        if (isInHand && !isChargingThrow)
        {
            //Put harpoon in the position of the seal
            transform.position = new Vector2(seal.transform.position.x + sealScript.harpoonOffset, seal.transform.position.y - 0.13f);
            startPosition = transform.position;

            //Rotate in the direction of the mouse position
            direction = (mouseOnScreen - positionOnScreen).normalized;
            transform.right = direction;
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
