using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HideBlock : TrapManager
{
    SpriteRenderer Sp;

    public override void Start()
    {
        base.Start();

        ResisterFold(ParentObject());

        Sp = GetComponent<SpriteRenderer>();

        Sp.color = new(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || TrapPlayed) //��������̑��x�ȏ�Œ@���΍쓮
            {
                //PlayerManager_Ray.Instance.ForceY(-1f, 0f);//���ɂ�����Ɛ�����t����
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����
                return;
            }

            TrapPlayed = true;//�g���b�v�쓮
            //Debug.Log("�R�C���I�I�I�I");

            AudioManager.Instance.PlayWithNotDuplication_SE(1);//���ʉ�

            PlayerManager_Ray.Instance.ForceY(-2f, 0f);//������Ƃ����ɒ@������

            gameObject.transform.DOLocalJump(Vector2.zero, 0.8f, 1, 0.2f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            Sp.color = new(1, 1, 1, 1);//���ߏ�Ԃ�����

            //�u���b�N��n�ʔ���ɕς���
            gameObject.transform.parent.gameObject.layer = LayerMask.NameToLayer("Ground");
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();
        
        Sp.color = new(1, 1, 1, 0);//�Ăѓ��ߏ�Ԃ�

        gameObject.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerMask.NameToLayer("Default");

        //Debug.Log("�܂��B��");
    }
}
