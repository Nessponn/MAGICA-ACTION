using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PushBlockRight : TrapManager
{
    public GameObject Block;
    [Range(1, 20f)] public float PushPower = 15f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (TrapPlayed) return;

            //時間差で吹っ飛ばす
            DOVirtual.DelayedCall(0.1f, () =>
            {
                //プレイヤーを弾き飛ばす
                //Debug.Log("吹き飛べ！");
                TrapPlayed = true;

                AudioManager.Instance.Play_SE(5);

                PlayerManager_Ray.Instance.ForceX(PushPower, 0.3f);
                PlayerManager_Ray.Instance.ForceY(5, 0.3f);

                //ParentObject().transform.DOLocalMoveX(-1, 0.1f).SetLoops(2, LoopType.Yoyo);
                Block.transform.DOLocalMoveX(1, 0.1f).SetLoops(2, LoopType.Yoyo);
            });

            DOVirtual.DelayedCall(1.5f, () =>
            {
                TrapPlayed = false;
            });
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();
    }
}
