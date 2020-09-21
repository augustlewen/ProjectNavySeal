using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    private GameObject harpoon;
    private HarpoonController harpoonScript;
    private Rigidbody2D myRigidbody;

    private Vector3 myStartPosition;

    private void Start()
    {
        myStartPosition = transform.position;

        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        myRigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Harpoon_Pull();
    }

    private void Harpoon_Pull()
    {
        if (harpoonScript.hasHit)
        {
            //Vector3 positionToMoveTowards = Vector3.zero;
            //Vector3 direction = Vector3.zero;

            //if (positionToMoveTowards == Vector3.zero)
            //{
            //    positionToMoveTowards = harpoon.transform.position;
            //    direction = (this.transform.position - harpoon.transform.position).normalized;
            //}

            myRigidbody.velocity = transform.position - harpoon.transform.position;

        }
    }
}
