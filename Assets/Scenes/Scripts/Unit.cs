using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void ReceiveDamage()
    {
        Die();
    }
}
