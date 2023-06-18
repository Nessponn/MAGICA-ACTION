using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackCloud : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            transform.parent.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            PlayerManager_Ray.Instance.Death_process();//‚Ô‚ÁŽE‚µ‚½c

            anim.SetBool("Thunder", true);

            DOVirtual.DelayedCall(1.5f, () =>
            {
                anim.SetBool("Thunder", false);
            }).SetLink(gameObject);
        }
    }

}
