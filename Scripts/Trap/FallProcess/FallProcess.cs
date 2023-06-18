using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallProcess : TrapManager
{
    //[Range(1, 5f)] public float distance = 3f;//�L�т��������鋗��
    [Range(1, 5f)] public float speed = 3;//�������x
    private Transform player;
    private Rigidbody2D rbody;

    public override void Start()
    {
        base.Start();
        player = PlayerManager_Ray.Instance.gameObject.transform;

        ResisterFold(gameObject);

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
        //�˒��������Ƀv���C���[����������A������
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
                rbody.constraints = RigidbodyConstraints2D.FreezeAll;//������x��������~�߂�
            });
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();

        transform.position = Firstpos;//�����ʒu�ɖ߂�
        

        //Debug.Log("�߂�");
    }
}
