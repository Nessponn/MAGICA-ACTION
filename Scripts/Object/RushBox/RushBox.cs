using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RushBox : TrapManager
{
    public GameObject obj;

    private Animator anim;

    //�A���ŃA�C�e�����o�Ă���

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
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || TrapPlayed) //��������̑��x�ȏ�Œ@���΍쓮
            {
                PlayerManager_Ray.Instance.ForceY(-1f, 0f);//���ɂ�����Ɛ�����t����
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����
                return;
            }

            TrapPlayed = true;//�쓮����

            PlayerManager_Ray.Instance.ForceY(-1f, 0f);//���ɂ�����Ɛ�����t����

            gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            anim.SetBool("Played", true);//�A�j���[�V����

            AudioManager.Instance.Play_SE(2);//���ʉ�

            GameObject Obj = Instantiate(obj, transform.position, Quaternion.identity);
            Obj.transform.DOLocalMoveY(Firstpos.y + 1, 0.15f);

            RushObject();//���Y�J�n
        }
    }

    //�����ɃI�u�W�F�N�g���o��
    void RushObject()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            AudioManager.Instance.Play_SE(2);//���ʉ�

            GameObject Obj = Instantiate(obj, transform.position, Quaternion.identity);
            Obj.transform.DOLocalMoveY(Firstpos.y + 1, 0.15f);

            if(TrapPlayed) RushObject();
        });
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();
        TrapPlayed = false;
        anim.SetBool("Played", false);
    }
}
