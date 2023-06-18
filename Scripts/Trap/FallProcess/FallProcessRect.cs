using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallProcessRect : TrapManager
{
    [Range(1, 5f)] public float speed = 3;//落下速度
    public float FirstYPos;
    private Rigidbody2D rbody;

    public override void Start()
    {
        //base.Start();
        //player = PlayerManager_Ray.Instance.gameObject.transform;

        Firstpos = gameObject.GetComponent<RectTransform>().localPosition;
/*
        Debug.Log("pos = " + gameObject.GetComponent<RectTransform>().position);
        Debug.Log("local = " + gameObject.GetComponent<RectTransform>().localPosition);


        Debug.Log("pos = " + (gameObject.GetComponent<RectTransform>().position == Firstpos));
        Debug.Log("local = " + (gameObject.GetComponent<RectTransform>().localPosition == Firstpos));
        Debug.Log("anchored = " + (gameObject.GetComponent<RectTransform>().anchoredPosition == new Vector2(Firstpos.x, Firstpos.y)));*/
        if (GetComponent<Rigidbody2D>()) rbody = GetComponent<Rigidbody2D>();
        else rbody = gameObject.AddComponent<Rigidbody2D>();

        rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        rbody.gravityScale = speed;
    }

    public void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //射程距離内にプレイヤーが入ったら、落ちる
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (TrapPlayed)
            {
                return;
            }
            TrapPlayed = true;

            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            rbody.velocity = new Vector2(0, -0.1f);

            DOVirtual.DelayedCall(1.5f, () =>
            {
                rbody.constraints = RigidbodyConstraints2D.FreezeAll;//ある程度落ちたら止める
            });
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();

        gameObject.GetComponent<RectTransform>().localPosition = Firstpos;//初期位置に戻す
        //Debug.Log("戻れ");
    }
}
