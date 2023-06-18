using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackObject : MonoBehaviour
{
    //�A�j���[�V�����𔭓�������Ƃ��́ASetbool��Played�ɂ��邱��

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            //transform.parent.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            PlayerManager_Ray.Instance.Death_process();//�Ԃ��E�����c

            anim.SetBool("Played", true);

            DOVirtual.DelayedCall(1.5f, () =>
            {
                anim.SetBool("Played", false);
            }).SetLink(gameObject);
        }
    }
}
