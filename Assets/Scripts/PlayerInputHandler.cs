using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 moveVal;
    public float speed = 2f;
    public float moveSpeed = 2f;
    public float crouchMalus = -1f;
    public float runModifier = 3.5f;
    public bool isSprinting = false;
    public bool isCrouching = false;
    public bool isGrounded = true;
    public Rigidbody rb;
    public float jumpHeight = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = moveSpeed;
    }

    private void Update()
    {
        float tmp = speed + (Convert.ToSingle(isSprinting) * runModifier) + Convert.ToSingle(isCrouching) * crouchMalus;
        //speed = tmp;
        transform.Translate(moveVal * tmp * Time.deltaTime);
        Debug.Log(tmp);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVal.x = value.ReadValue<Vector2>().x;
        moveVal.z = value.ReadValue<Vector2>().y;
    }

    public void OnLook()
    {
        Debug.Log("OnLook");
    }

    public void OnCrouch(InputAction.CallbackContext value)
    {
        isCrouching = value.performed;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        Debug.Log("OnJump");
        if (value.started && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        isSprinting = value.performed;
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        Debug.Log("OnInteract");
        if (value.started)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }

    public GameObject[] bulletPrefab = new GameObject[2];
    public int weaponIndex;
    
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            GameObject clone = Instantiate(bulletPrefab[weaponIndex]);
            clone.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
            clone.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 50, ForceMode.Impulse);
            Destroy(clone, 5f);
        }
        Debug.Log("OnAttack");
    }

    public void OnAltAttack(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            GameObject clone = Instantiate(bulletPrefab[weaponIndex]);
            clone.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
            clone.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 50, ForceMode.Impulse);
            Destroy(clone, 5f);
        }
        Debug.Log("OnAltAttack");
    }

    public void OnSwapWeapon(InputAction.CallbackContext value)
    {
        //weaponIndex += value.
        Debug.Log("OnSwapWeapon");
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}