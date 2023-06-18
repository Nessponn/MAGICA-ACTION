using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrinkBox : EnemyManager
{
    [Range(1, 5f)] public float distance = 3f;//L‚Ñ‚ª”­“®‚·‚é‹——£
    [Range(1, 20f)] public int scale = 4;
    public GameObject ShrinkingEnemy_PartMiddle;//L‚Ñ‚é‚¨• ‚Ì•”•ª
    public GameObject ShrinkingEnemy_PartHead;//L‚Ñ‚é‚¨K‚Ì•”•ª
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
            //Debug.Log("•Ç”»’è");
            rbody.velocity = new Vector2(-speed * dir, rbody.velocity.y);
        }
        else if (hit)
        {
            //Debug.Log("’n–Ê”»’è");
            rbody.velocity = new Vector2(-speed * dir, 0);
        }
        else
        {
            //Debug.Log("—‰º”»’è");
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

                //ÅŒãˆÈŠO‚Ü‚Å‚Í‚¨• ‚ğ¶¬
                if(i == scale - 1)
                {
                    GameObject obj =  Instantiate(ShrinkingEnemy_PartHead, Vector2.zero, Quaternion.identity);//ÅŒã‚Í“ª‚ğ¶¬
                    obj.transform.parent = transform;
                    obj.transform.localPosition = Vector2.zero;

                    obj.transform.DOLocalMoveY(vec.y, 0.2f).SetLink(obj);
                }
                else
                {
                    GameObject obj = Instantiate(ShrinkingEnemy_PartMiddle, Vector2.zero, Quaternion.identity);//ÅŒãˆÈŠO‚Í‚¨• ‚ğ¶¬
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

        PlayerManager_Ray.Instance.Death_process();//‚Ô‚ÁE‚µ‚½c
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
