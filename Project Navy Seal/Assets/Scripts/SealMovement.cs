using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    public float pullForce;
    public float moveSpeed;

    private GameObject harpoon;
    private HarpoonController harpoonScript;
    public Rigidbody2D myRigidbody;

    private Vector3 StartPosition;

    private void Start()
    {
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        myRigidbody = GetComponent<Rigidbody2D>();

        StartPosition = transform.position;
    }


    private void Update()
    {
        Harpoon_Pull();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
        if (mousePosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        else
            transform.localScale = new Vector3(1, 1, 1);

    }

    private void Harpoon_Pull()
    {
        if (harpoonScript.hasHit)
        {
            //myRigidbody.velocity = harpoon.transform.position - myStartPosition;
            myRigidbody.AddForce(harpoon.transform.position - StartPosition * pullForce, ForceMode2D.Impulse);

            harpoonScript.hasHit = false;
        }
    }
}
