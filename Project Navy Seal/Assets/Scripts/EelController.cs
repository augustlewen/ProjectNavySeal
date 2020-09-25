using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelController : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    private AudioSource electricity;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
        electricity = GetComponent<AudioSource>();
    }
    void Update()
    {
        transform.position = transform.position + transform.up * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Seal"))
        {
            electricity.Play();

            GameObject gameStartPosition = GameObject.FindGameObjectWithTag("Respawn");
            Rigidbody2D sealRigidibody = other.GetComponent<Rigidbody2D>();

            other.transform.position = gameStartPosition.transform.position;
            sealRigidibody.velocity = Vector2.zero;
        }
    }
}
