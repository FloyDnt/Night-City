using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : Unit
{
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private int lives = 5;
    private Vector3 direction;

    [SerializeField]
    private Transform cirTag;
    private float radCir = 0.3F;

    private LivesBar livesBar;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 5) lives = value;
            livesBar.Refresh();
        }
    }

    [SerializeField]
    private float jumpForce = 15.0F;

    private Bullet bullet;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (Input.GetButtonDown("Jump")) Jump();

    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        State = CharState.Run;
    }

    private void Jump()
    {
        if (CheckGround())
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            State = CharState.Jump;
        }
    }

    public override void ReceiveDamage()
    {
        Lives--;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 4F, ForceMode2D.Impulse);

        Debug.Log(lives);

        if (lives == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
    }

    bool CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(cirTag.position, radCir);

        int j = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders [i].gameObject != gameObject)
            {
                j++;
            }
        }
        return j > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit) ReceiveDamage();
    }
}

public enum CharState
{
    Idle,
    Run,
    Jump
}
