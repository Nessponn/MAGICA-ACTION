using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicBox_camouflage : TrapManager
{
    SpriteRenderer Sp;
    public Sprite music;
    private Sprite music_before;

    public int JumpPower = 120;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        ResisterFold(ParentObject());

        Sp = GetComponent<SpriteRenderer>();

        music_before = Sp.sprite;

        //Sp.color = new(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (TrapPlayed) return;

            TrapPlayed = true;//�g���b�v�쓮

            AudioManager.Instance.PlayWithNotDuplication_SE(1);//���ʉ�

            Sp.sprite = music;

            PlayerManager_Ray.Instance.ForceY(JumpPower, 1f);//��ɂԂ���΂�

            gameObject.transform.DOLocalJump(Vector2.zero, -0.8f, 1, 0.2f).SetEase(Ease.Linear);//�@���ꂽ�Ƃ��ɔ�яオ��A�j���[�V����

            GameMaster.Instance.CamLock(true);

            DOVirtual.DelayedCall(1f, () =>
            {
                PlayerManager_Ray.Instance.Fall_process();

            }).SetLink(gameObject);

            //Sp.color = new(1, 1, 1, 1);//���ߏ�Ԃ�����
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();

        //Sp.color = new(1, 1, 1, 0);//�Ăѓ��ߏ�Ԃ�

        if (music_before) Sp.sprite = music_before;
    }
}
