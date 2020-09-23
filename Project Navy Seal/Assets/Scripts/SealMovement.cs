using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    public float pullForce;
    public float moveSpeed;

    public bool isGrounded;

    private GameObject harpoon;
    private HarpoonController harpoonScript;
    public Rigidbody2D myRigidbody;


    private void Start()
    {
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        myRigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Harpoon_Pull();
        Turn_Around();


    }

    
    //Pull yourself Towards where the harpoon landed
    private void Harpoon_Pull()
    {
        if (harpoonScript.hasHit)
        {
            Vector2 directionForce = (harpoon.transform.position - transform.position);

            myRigidbody.AddForce(directionForce * pullForce, ForceMode2D.Impulse);

            harpoonScript.hasHit = false;
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
    private void OnCollisionEnter2D(Collision2D ground)
    {
        if (ground.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D ground)
    {
        if (ground.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = false;
    }

}
