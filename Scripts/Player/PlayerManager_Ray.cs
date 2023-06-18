using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager_Ray : SingletonMonoBehaviourFast<PlayerManager_Ray>
{
    [Range(0, 1f)] public float speed = 0.08f;//速度
    [Range(0.01f, 1f)] public float Friction = 0.05f;//無操作時の速度減衰係数
    [Range(7, 15)] public float MAXSpeed = 8f;//最大速度
    [Range(1f, 2f)] public float Landing_acceleration = 1.2f;//着地時の加速度
    [Range(15f, 40f)] public float JumpPower =25f;//ジャンプ力

    [HideInInspector] public Rigidbody2D rbody;//Rigidbody
    [HideInInspector] public Vector2 FirstPos;//初期位置、中間地点
    [HideInInspector] public bool Cleared;//クリアしたらtrue
    [HideInInspector] public SpriteRenderer Sp;//スプライトコンポーネント
    public LayerMask floorLayer;  //床レイヤー
    public LayerMask wallLayer;   //壁レイヤー
    public LayerMask enemyLayer;  //敵レイヤー
    public LayerMask ladderLayer; //はしご、豆の木レイヤー

    private float RockTime;//硬直時間
    private bool Landing;//着地しているか
    private bool Alive;//生存しているか
    private bool Ladder;//梯子、豆の木に捕まっているか
    private Animator anim;
    private bool ccc;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        FirstPos = gameObject.transform.position;

        Sp = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        DOVirtual.DelayedCall(2f, () =>
        {
            Alive = true;

            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            ForceY(-1, 0);
        }).SetLink(gameObject);
    }

    public void ReStart()
    {
        //Debug.Log("ななななな");

        rbody.constraints = RigidbodyConstraints2D.None;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        //判定を復活させる
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
        transform.position = FirstPos;//中間に戻す
        Alive = true;//生き返らせる

        //ゲームマネージャに再スタート処理を送る
        GameMaster.Instance.Restart_process();
        GameMaster.Instance.OpenAnimation();
        GameMaster.Instance.NotClearFlag();

        Cleared = false;


        anim.SetBool("Defeat", false);
        anim.SetBool("Walk", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!Alive) return;

        if (Cleared)
        {
            Landing_judgment();

            if(Landing)rbody.velocity = new Vector2(5, rbody.velocity.y); 
            else new Vector2(0, rbody.velocity.y);

            anim.SetBool("Walk", true);

            if (!ccc)
            {
                ccc = true;
                DOVirtual.DelayedCall(5f, () =>
                {
                    rbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }).SetLink(gameObject); ;
            }

            return;
        }

        if(RockTime > 0)
        {
            RockTime -= Time.deltaTime;
        }
        else
        {
            Move_processing();
            
        }

        if (!Ladder)Landing_judgment();

        if(rbody.velocity.y <= -18) rbody.velocity = new Vector2(rbody.velocity.x, -18);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Death_process();
        }
    }

    //クリア時の処理
    public void ClearFlag()
    {
        Cleared = true;

        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    //移動処理
    void Move_processing()
    {
        //壁用のRaycast
        //右
        Vector2 rightStart = transform.position + (transform.right * 0.55f) + (transform.up * 0.4f);
        Vector2 rightEnd = transform.position + (transform.right * 0.55f) + (transform.up * -0.1f);
        //左
        Vector2 leftStart = transform.position - (transform.right * 0.55f) + (transform.up * 0.4f);
        Vector2 leftEnd = transform.position - (transform.right * 0.55f) + (transform.up * -0.1f);

        //梯子用のRaycast
        Vector2 ladderStart = transform.position - (transform.up * 0.3f);
        Vector2 ladderEnd = transform.position - (transform.up * 0.3f);

        //Raycastを張る
        RaycastHit2D leftHit = Physics2D.Linecast(leftStart, leftEnd, wallLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);
        RaycastHit2D ladderHit = Physics2D.Linecast(ladderStart, ladderEnd, ladderLayer);

        //Raycastの座標デバッグ
        //Debug.DrawLine(rightStart, rightEnd, Color.green);//後で消す
        //Debug.DrawLine(leftStart, leftEnd, Color.red);//後で消す
        Debug.DrawLine(ladderStart, ladderEnd, Color.red);//後で消す

        if (Input.GetKey(KeyCode.Space) && Ladder)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rbody.velocity = new Vector2(rbody.velocity.x, JumpPower);
                Landing = false;

                Ladder = false;

                AudioManager.Instance.PlayWithNotDuplication_SE(4);

                //anim.SetBool("Jump", true);
            }
            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (ladderHit)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                Ladder = true;

                rbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
        {
            Ladder = false;

            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        if (!Ladder) Landing_processing();//地上や空中にいるときの処理
        else Ladder_processing();//梯子に捕まっているときの処理

        //壁に接触したか判断する
        if (rightHit) 
        {
            //Debug.Log("壁にあたった：右");
            if (rbody.velocity.x > 0f) rbody.velocity = new Vector2(0, rbody.velocity.y); 
        }
        if (leftHit)
        {
            //Debug.Log("壁にあたった：左");
            if (rbody.velocity.x < 0f) rbody.velocity = new Vector2(0, rbody.velocity.y); 
        }

    }

    void Landing_processing()
    {
        //床に接触してるか判断する

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Sp.flipX = false;
            anim.SetBool("Walk", true);

            if (!(rbody.velocity.x <= -MAXSpeed))
            {
                if (Landing) rbody.velocity += new Vector2(-speed, 0);
                else rbody.velocity += new Vector2(-speed / 1.8f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Sp.flipX = true;
            anim.SetBool("Walk", true);

            if (!(rbody.velocity.x >= MAXSpeed))
            {
                if (Landing) rbody.velocity += new Vector2(speed, 0);
                else rbody.velocity += new Vector2(speed / 1.8f, 0);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            if (Mathf.Abs(rbody.velocity.x) >= 0f || Mathf.Abs(rbody.velocity.x) >= MAXSpeed) rbody.velocity = new Vector2(rbody.velocity.x * (1f - Friction), rbody.velocity.y);
            if (rbody.velocity.x < 0.005f && rbody.velocity.x > -0.005f) rbody.velocity = new Vector2(0, rbody.velocity.y);
        }
    }

    void Ladder_processing()
    {
        float speedx = 0;
        float speedy = 0;
/*
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector2(0, 0.02f));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector2(0, -0.02f));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector2(-0.02f, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector2(0.02f, 0));
        }
*/
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            speedy = 0.02f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speedy = -0.02f;
        }
        else
        {
            speedy = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            speedx = -0.02f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            speedx = 0.02f;
        }
        else
        {
            speedx = 0;
        }

        transform.Translate(new Vector2(speedx, speedy));

    }

    //着地判定
    private bool EnemyStanping;

    void Landing_judgment()
    {
        //地面
        //始点と終点（Ray描写用。実際の判定ではない）
        Vector3 origin1 = transform.position + (transform.right * 0.35f) + (transform.up * -0.4f);
        Vector3 origin2 = transform.position - (transform.right * 0.35f) + (transform.up * -0.4f);
        Vector3 distance = Vector3.down * 0.3f;
        //実際のRayの判定
        RaycastHit2D hit1 = Physics2D.Raycast(origin1, Vector2.down, 0.3f, floorLayer);//地面
        RaycastHit2D hit2 = Physics2D.Raycast(origin2, Vector2.down, 0.3f, floorLayer);//地面

        //敵
        Vector3 origin_right = transform.position + (transform.right * 0.4f) + (transform.up * -0.4f);//右側の判定
        Vector3 origin_left = transform.position + (transform.right * -0.4f) + (transform.up * -0.4f);//左側の判定
        Vector3 distance_enemy = Vector3.down * 0.5f;
        RaycastHit2D hitEnemy1 = Physics2D.Raycast(origin_right, Vector2.down, 0.5f, enemyLayer);//敵1
        RaycastHit2D hitEnemy2 = Physics2D.Raycast(origin_left, Vector2.down, 0.5f, enemyLayer);//敵2

        //Debug.DrawRay(origin, distance, Color.yellow);
        //Debug.DrawRay(origin_right, distance_enemy, Color.yellow);
        //Debug.DrawRay(origin_left, distance_enemy, Color.yellow);

        if (hit1 && !(rbody.velocity.y > 1f))
        {
            Debug.DrawRay(origin1, distance, Color.yellow);
            transform.position = new Vector2(transform.position.x, hit1.point.y + 0.5f);

            if (!Landing && rbody.velocity.y <= -0.5f)
            {
                //着地時に加速
                float velx = rbody.velocity.x * Landing_acceleration;
                if (velx >= MAXSpeed) velx = MAXSpeed;
                else if (velx <= -MAXSpeed) velx = -MAXSpeed;

                rbody.velocity = new Vector2(velx, rbody.velocity.y);
                Landing = true;
                anim.SetBool("Jump", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rbody.velocity = new Vector2(rbody.velocity.x, JumpPower);
                Landing = false;

                AudioManager.Instance.PlayWithNotDuplication_SE(4);

                anim.SetBool("Jump", true);
            }
            else
            {
                rbody.velocity = new Vector2(rbody.velocity.x, 0);//ここを別のものに変えると坂を登れなくなる

                //たまにぐらぐらするのは、体重と重力係数が関係しているかも
            }
        }
        else if (hit2 && !(rbody.velocity.y > 1f))
        {
            Debug.DrawRay(origin2, distance, Color.yellow);
            transform.position = new Vector2(transform.position.x, hit2.point.y + 0.5f);

            if (!Landing && rbody.velocity.y <= -0.5f)
            {
                //着地時に加速
                float velx = rbody.velocity.x * Landing_acceleration;
                if (velx >= MAXSpeed) velx = MAXSpeed;
                else if (velx <= -MAXSpeed) velx = -MAXSpeed;

                rbody.velocity = new Vector2(velx, rbody.velocity.y);
                Landing = true;
                anim.SetBool("Jump", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rbody.velocity = new Vector2(rbody.velocity.x, JumpPower);
                Landing = false;

                AudioManager.Instance.PlayWithNotDuplication_SE(4);

                anim.SetBool("Jump", true);
            }
            else
            {
                rbody.velocity = new Vector2(rbody.velocity.x, 0);//ここを別のものに変えると坂を登れなくなる

                //たまにぐらぐらするのは、体重と重力係数が関係しているかも
            }
        }
        else
        {
            Debug.DrawRay(origin1, distance, Color.blue);
            Debug.DrawRay(origin2, distance, Color.blue);

            //宙にいれば、着地状態を解除
            if (Landing)
            {
                Landing = false;

                anim.SetBool("Jump", false);
            }
            //宙にいるときに、ボタンを離すことでジャンプをキャンセル
            if (Input.GetKeyUp(KeyCode.Space) && rbody.velocity.y > 3)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, 3);
            }

            //重量感のある動きを表現
            if (rbody.velocity.y > 0f)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y * 0.995f);
            }
            if (rbody.velocity.y <= -1f)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y * 0.999f);
            }
        }

        //敵を踏んだ時の処理
        if ((hitEnemy1 || hitEnemy2) && !Landing)
        {
            if (!EnemyStanping)
            {
                if (hitEnemy1)
                {
                    hitEnemy1.collider.gameObject.GetComponent<EnemyManager>().Defeat_Enemy();
                    EnemyStanping = true;

                   /* //敵にダメージを与える
                    if (hitEnemy1.collider.gameObject.GetComponent<EnemyManager>() && !hitEnemy2)
                    {
                        
                    }*/
                }
                else if (hitEnemy2)
                {
                    hitEnemy2.collider.gameObject.GetComponent<EnemyManager>().Defeat_Enemy();
                    EnemyStanping = true;

                    /*//敵にダメージを与える
                    if (hitEnemy2.collider.gameObject.GetComponent<EnemyManager>())
                    {
                        
                    }*/
                }

                if (!Landing)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        rbody.velocity = new Vector2(rbody.velocity.x, JumpPower * 1.2f);
                        Landing = false;
                    }
                    else
                    {
                        rbody.velocity = new Vector2(rbody.velocity.x, JumpPower / 2f);
                    }
                }
            }
        }
        else
        {
            EnemyStanping = false;
        }
    }
    public void AddForce(Vector2 vec)
    {
        rbody.velocity += vec;
    }

    public void ForceX(float num, float RockTime)
    {
        this.RockTime = RockTime;
        rbody.velocity = new Vector2(num, rbody.velocity.y);
    }

    public void ForceY(float num, float RockTime)
    {
        this.RockTime = RockTime;
        rbody.velocity = new Vector2(rbody.velocity.x, num);
    }

    //やられたときの処理
    public void Death_process()
    {
        if (!Alive) return;//既にやられている場合は実行しない

        Alive = false;//死ぬ

        anim.SetBool("Defeat", true);

        //BGMを止める
        AudioManager.Instance.Stop_BGM();

        //効果音を鳴らす
        AudioManager.Instance.Play_SE(0);

        //子オブジェクトのコライダーの判定もいったん消す
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //数秒止まって
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //残機減らす
        GameMaster.Instance.RestDecrease();

        //プレイヤーのスプライトを最前面に
        Sp.sortingOrder = 100;

        //高く飛び上がり、そのまま落下
        DOVirtual.DelayedCall(0.7f, () =>
        {
            rbody.constraints = RigidbodyConstraints2D.None;
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX;

            rbody.velocity = new Vector2(0, 22);
        }).SetLink(gameObject); ;

        DOVirtual.DelayedCall(1f, () =>
        {
            GameMaster.Instance.CloseAnimation();
        }).SetLink(gameObject); ;

        //カメラの下で数秒待機
        DOVirtual.DelayedCall(2.3f, () =>
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }).SetLink(gameObject); ;

        DOVirtual.DelayedCall(2.5f, () =>
        {
            //プレイヤーのスプライトを最前面から元に戻す
            Sp.sortingOrder = 10;

            //プレイヤーを生き返らせる
            ReStart();

        }).SetLink(gameObject); ;

        //GameMaster.Instance.Restart_process();
    }

    //落下死の処理
    public void Fall_process()
    {
        if (!Alive) return;//既にやられている場合は実行しない

        Alive = false;//死ぬ

        //BGMを止める
        AudioManager.Instance.Stop_BGM();

        //残機減らす
        GameMaster.Instance.RestDecrease();

        //効果音を鳴らす
        AudioManager.Instance.Play_SE(0);

        DOVirtual.DelayedCall(1f, () =>
        {
            GameMaster.Instance.CloseAnimation();
        }).SetLink(gameObject);

        //カメラの下で数秒待機
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        DOVirtual.DelayedCall(2.5f, () =>
        {
            //Debug.Log("ぬぬぬぬぬ");
            //プレイヤーを生き返らせる
            ReStart();
        }).SetLink(gameObject);

        //GameMaster.Instance.Restart_process();
    }
}
