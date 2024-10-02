using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Animator animator;

    [SerializeField] float movePower;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxFallSpeed;

    [SerializeField] bool isGrounded;

    private float x;

    private bool life = true;

    private static int idleHash = Animator.StringToHash("small_mario_idle");
    private static int runHash = Animator.StringToHash("small_mario_run");
    private static int jumpHash = Animator.StringToHash("small_mario_jump");
    private static int dieHash = Animator.StringToHash("small_mario_die");

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        Jump();

        Run();

        AnimatorPlay();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void FixedUpdate()
    {
        Move();
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

    private void AnimatorPlay()
    {
        if (life == false)
        {
            animator.Play(dieHash);
        }
        else if (rigid.velocity.y > 0.01f)
        {
            animator.Play(jumpHash);
        }
        else if (rigid.velocity.y < -0.01f)
        {
            animator.Play(jumpHash);
        }
        else if(rigid.velocity.sqrMagnitude < 0.01f)
        {
            animator.Play(idleHash);
        }
        else
        {
            animator.Play(runHash);
        }
    }

    public void Death()
    {
        life = false;

        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        if (TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.isKinematic = true;
        }
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
