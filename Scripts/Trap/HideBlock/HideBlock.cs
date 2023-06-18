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
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || TrapPlayed) //下から一定の速度以上で叩けば作動
            {
                //PlayerManager_Ray.Instance.ForceY(-1f, 0f);//下にちょっと勢いを付ける
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション
                return;
            }

            TrapPlayed = true;//トラップ作動
            //Debug.Log("コイン！！！！");

            AudioManager.Instance.PlayWithNotDuplication_SE(1);//効果音

            PlayerManager_Ray.Instance.ForceY(-2f, 0f);//ちょっとしたに叩き落す

            gameObject.transform.DOLocalJump(Vector2.zero, 0.8f, 1, 0.2f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション

            Sp.color = new(1, 1, 1, 1);//透過状態を解除

            //ブロックを地面判定に変える
            gameObject.transform.parent.gameObject.layer = LayerMask.NameToLayer("Ground");
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();
        
        Sp.color = new(1, 1, 1, 0);//再び透過状態へ

        gameObject.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerMask.NameToLayer("Default");

        //Debug.Log("また隠す");
    }
}
