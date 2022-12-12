using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonstar : Monster
{
    [SerializeField]
    private float rate = 2.0F;

    private Vector3 direction;
    private SpriteRenderer sprite;
    private Transform target;

    private RedBullet Redbullet;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Character").transform;
        InvokeRepeating("Shoot", rate, rate);
    }
    protected override void Update()
    {
        direction = target.position - transform.position;
    }
    protected override void Awake()
    {
        Redbullet = Resources.Load<RedBullet>("RedBullet");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    private void Shoot()
    {
        Vector3 position = transform.position; position.x += 0.7F; position.y += 0.2F;
        RedBullet RednewBullet = Instantiate(Redbullet, position, Redbullet.transform.rotation) as RedBullet;

        RednewBullet.RedParent = gameObject;
        if (direction.x > 0)
        {
            RednewBullet.Direction = RednewBullet.transform.right;
            sprite.flipX = direction.x > 0.0F;
        }
        else
        {         
            RednewBullet.Direction = -RednewBullet.transform.right;
            sprite.flipX = direction.x > 0.0F;
        }


    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();

        if (bullet)
        {
            ReceiveDamage();
        }
    }
}
