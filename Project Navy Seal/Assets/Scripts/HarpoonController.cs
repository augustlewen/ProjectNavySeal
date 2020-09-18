using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HarpoonController : MonoBehaviour
{
    public bool inHand = true;
    public bool chargingThrow;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Rotate_Harpoon();
        Throw_Harpoon();
    }

    private void Rotate_Harpoon()
    {
        if (inHand && chargingThrow == false)
        {
            //Get the screen position of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);


            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -angle));
        }
    }

    private void Throw_Harpoon()
    {
        //if(chargingThrow == false)
        if (Input.GetButton("Throw"))
        {
            chargingThrow = true;
            transform.position = Vector3.Lerp(transform.position, startPosition - transform.right, Time.time * 0.001f);
        }

        if (Input.GetButtonUp("Throw"))
        {
            chargingThrow = false;
            inHand = false;
        }




        if (chargingThrow == false && transform.position != startPosition)
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.time * 0.003f);

    }

    float AngleBetweenTwoPoints(Vector3 firstPoint, Vector3 secondPoint)
    {
        return Mathf.Atan2(firstPoint.y - secondPoint.y, secondPoint.x - firstPoint.x) * Mathf.Rad2Deg;
    }

}
