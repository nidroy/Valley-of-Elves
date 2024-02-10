using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;
    private bool isPickup;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPickup = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isPickup = false;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;

        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
    }

    private void FixedUpdate()
    {
        anim.SetBool("isPickup", isPickup);

        if (!isPickup)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Pickup()
    {
        
    }
}
