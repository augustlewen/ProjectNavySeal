using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [NonSerialized] public bool moving;
    [NonSerialized] public float horizontal;
    [NonSerialized] public bool squish;

    public float pullForce;
    public float moveSpeed;

    public bool isGrounded;

    private GameObject harpoon;
    private HarpoonController harpoonScript;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;


    private void Start()
    {
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        Harpoon_Pull();

        Movement();
        Turn_Around();

        Animation();
    }

    
    //Pull yourself Towards where the harpoon landed
    private void Harpoon_Pull()
    {
        if (harpoonScript.hasHit)
        {
            float distance = Vector2.Distance(harpoon.transform.position, transform.position);

            Vector2 directionForce = (harpoon.transform.position - transform.position).normalized;

            if(distance > 1.5)
            {
                myRigidbody.AddForce(directionForce * pullForce, ForceMode2D.Impulse);
                squish = true;
            }

            harpoonScript.hasHit = false;
        }
    }



    private void Movement()
    {
        if(horizontal != 0)
        {
            if (!harpoonScript.isChargingThrow && harpoonScript.isInHand && isGrounded)
            {
                myRigidbody.velocity = new Vector2(moveSpeed * horizontal, myRigidbody.velocity.y);

            }
        }
        
    }


    //Flips the sprite in the direction the harpoon is aiming
    private void Turn_Around()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
        if (mousePosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
            transform.localScale = new Vector3(1, 1, 1);
    }




    //Check if Seal is touching the ground
    private void OnCollisionStay2D(Collision2D ground)
    {
        if (ground.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D ground)
    {
        if (ground.gameObject.layer == LayerMask.NameToLayer("Ground") && ground.transform.position.y < transform.position.y)
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            squish = false;
        }
    }

    private void OnCollisionExit2D(Collision2D ground)
    {
        if (ground.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = false;
    }




    private void Animation()
    {
        myAnimator.SetBool("Squish", squish);
    }
}
