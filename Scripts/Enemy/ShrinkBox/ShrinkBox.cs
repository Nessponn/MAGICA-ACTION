using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrinkBox : EnemyManager
{
    [Range(1, 5f)] public float distance = 3f;//伸びが発動する距離
    [Range(1, 20f)] public int scale = 4;
    public GameObject ShrinkingEnemy_PartMiddle;//伸びるお腹の部分
    public GameObject ShrinkingEnemy_PartHead;//伸びるお尻の部分
    public Sprite ShrinkingEnemy_PartTail;
    public Sprite ShrinkingEnemy_Normal;

    private Transform player;
    private bool TrapPlayed;
    private int Count;
    Vector2 pos;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        player = PlayerManager_Ray.Instance.gameObject.transform;

        Sp = ChildObject(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

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

        //var col = GetComponent<BoxCollider2D>().offset;

        //4 - (-2) = 6
        //3 - (-1) = 4
        if (transform.position.x - player.position.x <= distance && !TrapPlayed)
        {
            TrapPlayed = true;

            Vector2 vec = Vector2.zero;

            Sp.sprite = ShrinkingEnemy_PartTail;

            for(int i = 0;i < scale; i++)
            {
                vec.y += 1;

                //最後以外まではお腹を生成
                if(i == scale - 1)
                {
                    GameObject obj =  Instantiate(ShrinkingEnemy_PartHead, Vector2.zero, Quaternion.identity);//最後は頭を生成
                    obj.transform.parent = transform;
                    obj.transform.localPosition = Vector2.zero;

                    obj.transform.DOLocalMoveY(vec.y, 0.2f).SetLink(obj);
                }
                else
                {
                    GameObject obj = Instantiate(ShrinkingEnemy_PartMiddle, Vector2.zero, Quaternion.identity);//最後以外はお腹を生成
                    obj.transform.parent = transform;
                    obj.transform.localPosition = Vector2.zero;

                    obj.transform.DOLocalMoveY(vec.y, 0.2f).SetLink(obj);
                }
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Reset_process()
    {
        base.Reset_process();

        if (TrapPlayed)
        {
            for (int i = 1; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            Sp.sprite = ShrinkingEnemy_Normal;
            TrapPlayed = false;
        }
    }

    public override void Defeat_Enemy()
    {
        
        
        gameObject.SetActive(false);
    }

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//ぶっ殺した…
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            Defeat_Player();
        }

    }
}
