using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameObject harpoon;
    private HarpoonController harpoonScript;

    private void Start()
    {
        
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();
    }

    void Update()
    {
        Throw_Harpoon();
        
    }

    private void Throw_Harpoon()
    {
        if(harpoonScript.isInHand)
        {
            //Hold down Throw button to build up force
            if (Input.GetButton("Throw"))
            {
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

                //Calculate Force
                harpoonScript.myRigidbody.AddForce(harpoon.transform.right * harpoonScript.force);
                harpoonScript.myRigidbody.gravityScale = 0.5f;
            }
        }
    }
}
