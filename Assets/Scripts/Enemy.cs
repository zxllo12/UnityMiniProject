using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;

    Vector2 velocity;

    private static int runHash = Animator.StringToHash("enemy_run");
    private static int dieHash = Animator.StringToHash("enemy_die");

    bool life = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            if (life == true)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            else
            {
                //player.Hit();
            }
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        life = true;
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        Destroy(gameObject, 3f);
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enabled = false;
    }
    private void OnBecameVisible()
    {
        #if UNITY_EDITOR
        enabled = !EditorApplication.isPaused;
        #else
        enabled = true;
        #endif
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigid.WakeUp();
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        rigid.Sleep();
    }
    private void Update()
    {
        AnimatorPlay();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + velocity * Time.fixedDeltaTime);

        if (rigid.Raycast(direction))
        {
            direction = -direction;
        }

        if (rigid.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }

        if (direction.x > 0f)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (direction.x < 0f)
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }

    private void AnimatorPlay()
    {
        if (life == true)
        {
            animator.Play(dieHash);
        }
        else
        {
            animator.Play(runHash);
        }
    }
}
