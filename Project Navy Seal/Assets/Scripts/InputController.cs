using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameObject harpoon;
    private HarpoonController harpoonScript;

    private GameObject seal;
    private SealMovement sealMovement;

    public float chargeTime;

    private void Start()
    {
        harpoon = GameObject.FindGameObjectWithTag("Harpoon");
        harpoonScript = harpoon.GetComponent<HarpoonController>();

        seal = GameObject.FindGameObjectWithTag("Seal");
        sealMovement = seal.GetComponent<SealMovement>();
    }

    void Update()
    {
        harpoonScript.chargeHarpoon = Input.GetButton("Throw");
        harpoonScript.throwHarpoon = Input.GetButtonUp("Throw");
        harpoonScript.recoverHarpoon = Input.GetButtonUp("Recover");

        sealMovement.horizontal = Input.GetAxis("Horizontal");
    }
}
