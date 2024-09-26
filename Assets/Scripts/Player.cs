using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;

    [SerializeField] float movePower;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxFallSpeed;

    [SerializeField] bool isGrounded;

    private float x;

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        Idle();

        Jump();

        Run();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Idle()
    {
        if (rigid.velocity.sqrMagnitude < 0.01f)
        {
            isGrounded = true;
        }
    }

    private void Move()
    {
        rigid.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);
        if (rigid.velocity.x > maxMoveSpeed)
        {
            rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxMoveSpeed)
        {
            rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y);
        }

        if (rigid.velocity.y < -maxFallSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallSpeed);
        }

        if (x < 0)
        {
            render.flipX = true;
        }
        else if (x > 0)
        {
            render.flipX = false;
        }

        if (rigid.velocity.sqrMagnitude > 0.01f)
        {
            isGrounded = true;
        }

    }
    private void Jump()
    {
        if (isGrounded == false)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (rigid.velocity.y > 0.01f)
        {
            isGrounded = false;
        }
        else if (rigid.velocity.y < -0.01f)
        {
            isGrounded = false;
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            maxMoveSpeed = 10f;
        }
        else
        {
            maxMoveSpeed = 5f;
        }
    }
}
