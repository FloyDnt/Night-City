using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullet : Unit
{
    private GameObject Redparent;
    public GameObject RedParent { set { Redparent = value; } }

    private float speed = 10.0F;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 0.4F);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            Debug.Log("aaaa");
            unit.ReceiveDamage();
        }
    }
}
