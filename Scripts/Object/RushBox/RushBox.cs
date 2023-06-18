using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RushBox : TrapManager
{
    public GameObject obj;

    private Animator anim;

    //連続でアイテムが出てくる

    public override void Start()
    {
        base.Start();

        anim = gameObject.GetComponent<Animator>();
    }
    //ハテナブロック
    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || TrapPlayed) //下から一定の速度以上で叩けば作動
            {
                PlayerManager_Ray.Instance.ForceY(-1f, 0f);//下にちょっと勢いを付ける
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション
                return;
            }

            TrapPlayed = true;//作動した

            PlayerManager_Ray.Instance.ForceY(-1f, 0f);//下にちょっと勢いを付ける

            gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション

            anim.SetBool("Played", true);//アニメーション

            AudioManager.Instance.Play_SE(2);//効果音

            GameObject Obj = Instantiate(obj, transform.position, Quaternion.identity);
            Obj.transform.DOLocalMoveY(Firstpos.y + 1, 0.15f);

            RushObject();//処刑開始
        }
    }

    //無限にオブジェクトを出す
    void RushObject()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            AudioManager.Instance.Play_SE(2);//効果音

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
