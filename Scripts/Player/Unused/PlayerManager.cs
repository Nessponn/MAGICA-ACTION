using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動や当たり判定の処理の総括を行う
public class PlayerManager : SingletonMonoBehaviourFast<PlayerManager>
{
    public Rigidbody2D rbody;

    [Range(0, 0.15f)] public float speed = 0.08f;


    private float MAXSpeed = 7f;

    GameObject cam;

    [HideInInspector]public bool Jump;

    [HideInInspector] public bool Wall ;

    float currentSpeed;

    float RockTime;//硬直時間


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(RockTime > 0)
        {
            RockTime -= Time.deltaTime;
        }
        else
        {
            if (rbody.velocity.x <= -MAXSpeed)//左に加速し過ぎた場合、減速
            {
                rbody.velocity += new Vector2(speed, 0);
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    //左に進行する

                    MoveLeft();
                }
            }

            if (rbody.velocity.x >= MAXSpeed)//右に加速し過ぎた場合、減速
            {
                rbody.velocity += new Vector2(-speed, 0);
            }
            else//Maxスピード以下であれば、操作を受け付ける
            {
                if (Input.GetKey(KeyCode.D))
                {
                    //右に進行する

                    MoveRight();
                }
            }

            /*if (Input.GetKey(KeyCode.A))
            {
                //左に進行する

                if (!(rbody.velocity.x <= -7)) MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //右に進行する

                if (!(rbody.velocity.x >= 7)) MoveRight();
            }*/

            if (Input.GetKey(KeyCode.Space) && Jump)
            {
                JumpUp();//ジャンプボタンを押す
            }
            else if (rbody.velocity.y >= 3 && Input.GetKeyUp(KeyCode.Space))
            {
                JumpDown();//ジャンプ中にボタンを離す
            }

            /*//地面にいるとき、地面に吸い付くようすることで、走っている途中に浮いてしまう現象を防止
            if (Jump)
            {
                if (rbody.velocity.y > 0f)
                {
                    Debug.Log("浮くな！");
                    float num = rbody.velocity.y;

                    rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y - num);
                }
            }*/
        }

        /*if (!Jump)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y * 0.95f);
        }*/



        cam.transform.position = new Vector3(gameObject.transform.position.x, 0, -10);

        //速度を保存
        currentSpeed = rbody.velocity.x;

    }

    private void MoveLeft()
    {
        //左に動く
        if(Jump)rbody.velocity += new Vector2(-speed, 0);//地上
        else rbody.velocity += new Vector2(-speed/2, 0);//空中

        //Tilemapの判定に突っかかる不具合への対処
        if (Mathf.Abs(rbody.velocity.x) <= 0.09f && currentSpeed < -0.07f && !Wall)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y +0.01f);
            rbody.velocity = new Vector2(currentSpeed, rbody.velocity.y);
            Debug.Log("突っかかった");
        }
    }

    private void MoveRight()
    {
        //右に動く
        if (Jump) rbody.velocity += new Vector2(speed, 0);
        else rbody.velocity += new Vector2(speed/2, 0);

        //Tilemapの判定に突っかかる不具合への対処
        if (Mathf.Abs(rbody.velocity.x) <= 0.09f && currentSpeed > 0.07f && !Wall)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
            rbody.velocity = new Vector2(currentSpeed, rbody.velocity.y);
            Debug.Log("突っかかった");
        }
    }

    //ジャンプ時の処理
    public void JumpUp()
    {
        PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.None;
        PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Jump = false;

        rbody.velocity = new Vector2(rbody.velocity.x, 20);
    }
    //ジャンプ中にジャンプボタンを離す
    public void JumpDown()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, 3);
    }

    //着地時の処理
    public void Ground()
    {
        rbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        Jump = true;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.015f);

        if (Mathf.Abs(rbody.velocity.x) > MAXSpeed - 4)
        {
            //Debug.Log("加速！");
            rbody.velocity = new Vector2(rbody.velocity.x * 1.8f, rbody.velocity.y);
        }
    }

    private void ForAir()
    {
        
    }

    public void AddForce(Vector2 vec)
    {
        rbody.velocity += vec;
    }

    public void ForceX(float num, float RockTime)
    {
        this.RockTime = RockTime;
        rbody.velocity = new Vector2(num , rbody.velocity.y);
    }

    public void ForceY(float num, float RockTime)
    {
        if (num >= 5) Jump = false;
        this.RockTime = RockTime;
        rbody.velocity = new Vector2(rbody.velocity.x, num);
    }
}
