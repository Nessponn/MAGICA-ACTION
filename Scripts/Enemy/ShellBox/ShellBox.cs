using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBox : EnemyManager
{
    public Sprite ShellIn;
    private Sprite ShellOut;

    private bool Stamped;//踏まれているか
    private bool GOGO;//亀の甲羅が暴れているか

    //private int playerdir = 0;

    [Range(1, 20f)] public float Shellspeed = 10f;//速度

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        ShellOut = Sp.sprite;

        //Debug.Log(ShellOut);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Stamped)
        {
            if (GOGO)
            {
                rbody.velocity = new Vector2(-speed * Shellspeed * dir, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(0, rbody.velocity.y);
            }

            if (hit)
            {
                //Debug.Log("地面判定");
                rbody.velocity = new Vector2(rbody.velocity.x, 0);
            }
        }
        else
        {
            if (leftHit || rightHit)
            {
                //Debug.Log("壁判定");
                rbody.velocity = new Vector2(-speed * dir, rbody.velocity.y);
            }
            else if (hit)
            {
                //Debug.Log("地面判定");
                rbody.velocity = new Vector2(-speed * dir, 0);
            }
            else
            {
                //Debug.Log("落下判定");
                rbody.velocity = new Vector2(-speed * dir, rbody.velocity.y);
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //Debug.Log("再び参上");

        if (Stamped) gameObject.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        //Debug.Log("消えます。さようなら");

        //Stamped = false;
        //GOGO = false;
        if (ShellOut) Sp.sprite = ShellOut;
        if (Sp) Sp.flipX = false;
        dir = 1;

        if (FirstPos != new Vector2(0, 0)) gameObject.transform.position = FirstPos;
    }

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        if (!Stamped) PlayerManager_Ray.Instance.Death_process();//ぶっ殺した…
        else 
        {
            if (GOGO) PlayerManager_Ray.Instance.Death_process();
            else
            {
                //亀を蹴った時の音
                AudioManager.Instance.Play_SE(8);

                if (PlayerManager_Ray.Instance.Sp.flipX) dir = -1;
                else dir = 1;
                GOGO = true;
            }
        }
    }

    public override void Defeat_Enemy()
    {
        //base.Defeat_Enemy();

        //gameObject.SetActive(false);

        if (Stamped)
        {
            //踏んだ効果音
            AudioManager.Instance.Play_SE(7);
            GOGO = !GOGO;

            //プレイヤーの向いている方向を取得する
            if (PlayerManager_Ray.Instance.Sp.flipX) dir = -1;
            else dir = 1;
        }

        if (!Stamped)
        {
            //踏んだ効果音
            AudioManager.Instance.Play_SE(7);

            Sp.sprite = ShellIn;
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            Stamped = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            Defeat_Player();
        }
        if ((layername == "Enemy" || layername == "EnemyItem") && GOGO)
        {
            //亀を蹴った時の音
            AudioManager.Instance.PlayWithNotDuplication_SE(8);
            col.gameObject.GetComponent<EnemyManager>().Defeat_Enemy();
        }
    }

    public override void Reset_process()
    {
        Stamped = false;
        GOGO = false;

        if (ShellOut) Sp.sprite = ShellOut;
        base.Reset_process();

        //Debug.Log("リセット！！！！");

    }
}
