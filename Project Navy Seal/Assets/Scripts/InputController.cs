using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameObject harpoon;
    private HarpoonController harpoonScript;

    private GameObject seal;
    private SealMovement sealMovement;

    private void Start()
    {
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        seal = GameObject.FindGameObjectWithTag("Seal");
        sealMovement = seal.GetComponent<SealMovement>();
    }

    void Update()
    {
        Throw_Harpoon();
        Recover_Harpoon();
        Movement();
    }



    private void Throw_Harpoon()
    {
        if(harpoonScript.isInHand)
        {

            if(!harpoonScript.isInvalidArea && sealMovement.isGrounded)
            {

                //Hold down Throw button to build up force
                if (Input.GetButton("Throw"))
                {
                    sealMovement.myRigidbody.velocity = new Vector2(0f, sealMovement.myRigidbody.velocity.y);

                    if (harpoonScript.force < harpoonScript.maxForce)
                    {
                        harpoonScript.isChargingThrow = true;

                        //Builds up Force over time
                        harpoonScript.force += Mathf.Lerp(0f, harpoonScript.maxForce, 0.002f);

                        harpoon.transform.position = Vector3.Lerp(harpoon.transform.position, harpoonScript.startPosition - harpoon.transform.right / 2.5f, 0.002f);
                    }
                }

                //Release Throw button to throw the harpoon with the force that you've built up
                if (Input.GetButtonUp("Throw"))
                {
                    harpoonScript.isChargingThrow = false;
                    harpoonScript.isInHand = false;
                    harpoonScript.isAirborne = true;

                    //Calculate Force
                    harpoonScript.myRigidbody.AddForce(harpoon.transform.right * harpoonScript.force);
                    harpoonScript.myRigidbody.gravityScale = 0.6f;
                }


            }
        }
    }


    private void Recover_Harpoon()
    {
        if (!harpoonScript.isInHand)
        {

            //Hold down Throw button to build up force
            if (Input.GetButtonDown("Recover"))
            {
                harpoonScript.isInHand = true;

                harpoonScript.startPosition = transform.position;

                harpoonScript.force = 0f;
                harpoonScript.ropePrefab.enabled = true;
                harpoonScript.myRigidbody.isKinematic = false;
                harpoonScript.myRigidbody.gravityScale = 0f;

            }
        }
    }


    private void Movement()
    {
        if(!harpoonScript.isChargingThrow && harpoonScript.isInHand && sealMovement.isGrounded)
        {
            
            float horizontal = Input.GetAxis("Horizontal");


            sealMovement.myRigidbody.velocity = new Vector2(sealMovement.moveSpeed * horizontal, sealMovement.myRigidbody.velocity.y);
        }
    }
}
