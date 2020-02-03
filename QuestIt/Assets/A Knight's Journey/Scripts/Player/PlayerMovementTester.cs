using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementTester : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 25f)]
    float moveSpeed = 2.0f;

    [SerializeField]
    [Range(2, 30)]
    float rotationSpeed = 5f;

    float horizontal;

    float vertical;

    public Animator myAnimator;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(vertical) > 0)
        {
            transform.position += transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        }

        if (Mathf.Abs(horizontal) > 0)
        {
            transform.Rotate(new Vector3(0, horizontal * rotationSpeed, 0));
        }

        myAnimator.SetFloat("Speed", vertical);
    }
}
