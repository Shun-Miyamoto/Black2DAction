using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEffect : MonoBehaviour, IDrainable
{
    [SerializeField] private int power = 1;
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.Rebind();//表示されたら最初から再生できるように
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            IDamageable idamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (idamageable != null)
            {
                idamageable.Damage(power);
            }
        }
    }

    public bool Drain()
    {
        return true;
    }
    public bool SuperDrain()
    {
        return true;
    }
}
