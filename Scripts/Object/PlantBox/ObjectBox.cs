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
    //ハテナブロック
    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y <= 1f || ObjectPlayed) //下から一定の速度以上で叩けば作動
            {
                PlayerManager_Ray.Instance.ForceY(-1f, 0f);//下にちょっと勢いを付ける
                gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション
                return;
            }

            ObjectPlayed = true;//作動した

            PlayerManager_Ray.Instance.ForceY(-1f, 0f);//下にちょっと勢いを付ける

            gameObject.transform.DOLocalJump(Vector2.zero, 0.4f, 1, 0.1f).SetEase(Ease.Linear);//叩かれたときに飛び上がるアニメーション

            anim.SetBool("Played", true);//アニメーション

            DOVirtual.DelayedCall(0.1f, () =>
            {
                AudioManager.Instance.PlayWithNotDuplication_SE(2);//効果音

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