using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicBox : TrapManager
{
    SpriteRenderer Sp;
    //public Sprite music;

    public int JumpPower = 40;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Sp = GetComponent<SpriteRenderer>();

        Sp.color = new(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y > 3f) return;

            //TrapPlayed = true;//�g���b�v�쓮

            AudioManager.Instance.PlayWithNotDuplication_SE(1);//���ʉ�

            PlayerManager_Ray.Instance.ForceY(JumpPower, 1f);//��ɂԂ���΂�

            gameObject.transform.DOLocalJump(Vector2.zero, -0.8f, 1, 0.2f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            Sp.color = new(1, 1, 1, 1);//���ߏ�Ԃ�����

            GameMaster.Instance.CamLock(true);

            DOVirtual.DelayedCall(1f, () =>
            {
                PlayerManager_Ray.Instance.Fall_process();

            }).SetLink(gameObject);
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();

        Sp.color = new(1, 1, 1, 0);//�Ăѓ��ߏ�Ԃ�
    }
}
