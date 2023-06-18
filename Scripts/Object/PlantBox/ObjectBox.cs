using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectBox : ObjectManager
{
    public GameObject obj;

    private Animator anim;

    public override void Start()
    {
        base.Start();

        anim = gameObject.GetComponent<Animator>();
    }
    //�n�e�i�u���b�N
    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || ObjectPlayed) //��������̑��x�ȏ�Œ@���΍쓮
            {
                PlayerManager_Ray.Instance.ForceY(-1f, 0f);//���ɂ�����Ɛ�����t����
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����
                return;
            }

            ObjectPlayed = true;//�쓮����

            PlayerManager_Ray.Instance.ForceY(-1f, 0f);//���ɂ�����Ɛ�����t����

            gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            anim.SetBool("Played", true);//�A�j���[�V����

            DOVirtual.DelayedCall(0.1f, () =>
            {
                AudioManager.Instance.PlayWithNotDuplication_SE(2);//���ʉ�

                Instantiate(obj, transform.position, Quaternion.identity);
            });
        }
    }
    public override void Object_Reset()
    {
        base.Object_Reset();

        anim.SetBool("Played", false);
    }
}