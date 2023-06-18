using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicalBox : ObjectManager
{
    private Animator anim;

    public override void Start()
    {
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

            gameObject.transform.DOLocalJump(Vector2.zero, 0.8f,1,0.2f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            AudioManager.Instance.PlayWithNotDuplication_SE(1);//���ʉ�

            anim.SetBool("Played", true);//�A�j���[�V����
        }
    }
    public override void Object_Reset()
    {
        base.Object_Reset();

        anim.SetBool("Played",false);
    }
}
