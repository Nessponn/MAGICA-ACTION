using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [Range(0, 1f)] public float speed = 0.5f;//速度
    public LayerMask floorLayer;  //床レイヤー
    public LayerMask wallLayer;   //壁レイヤー

    protected Rigidbody2D rbody;
    protected RaycastHit2D hit;//着地判定
    protected RaycastHit2D leftHit;//左の壁の接触判定
    protected RaycastHit2D rightHit;//右の壁の接触判定

    protected Vector2 FirstPos;//初期位置
    protected SpriteRenderer Sp;
    protected bool Landing;//着地しているか
    [HideInInspector] public int dir = 1;//進んでいる方向。1なら左、-1なら右

    [HideInInspector]public bool Spawned;//出現したか

    // Start is called before the first frame update
    public virtual void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        //始点と終点（Ray描写用。実際の判定ではない）
        Vector3 origin = transform.position;

        //実際のRayの判定
        hit = Physics2D.Raycast(origin, Vector2.down, 0.8f, floorLayer);

        Spawned = true;//出現した

        Sp = GetComponent<SpriteRenderer>();

        gameObject.SetActive(true);
    }

    public virtual void ResisterPosition()
    {
        FirstPos = gameObject.transform.position;//初期位置の登録
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //rbody.velocity = new Vector2(-0.5f, rbody.velocity.y);

        if(rbody)Landing_judgment();//地面の判定

        wall_judgment();//壁の判定
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    void Landing_judgment()
    {
        //実際のRayの判定
        Vector3 origin = transform.position;//始点
        Vector3 distance = Vector3.down * 0.8f;
        hit = Physics2D.Raycast(origin, Vector2.down, 0.8f, floorLayer);

        Debug.DrawRay(origin, distance, Color.yellow);

        if (hit)
        {
            transform.position = new Vector2(transform.position.x, hit.point.y + 0.5f);
            //rbody.velocity = new Vector2(rbody.velocity.x, 0);
            Landing = true;
        }
        else
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y);
            Landing = false;
        }
    }

    void wall_judgment()
    {
        //右
        Vector2 rightStart = transform.position + (transform.right * 0.55f) - (transform.up * 0.2f);
        Vector2 rightEnd = transform.position + (transform.right * 0.55f) - (transform.up * 0.1f);
        //左
        Vector2 leftStart = transform.position - (transform.right * 0.55f) - (transform.up * 0.2f);
        Vector2 leftEnd = transform.position - (transform.right * 0.55f) - (transform.up * 0.1f);
        //Raycastを張る
        RaycastHit2D leftHit = Physics2D.Linecast(leftStart, leftEnd, wallLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);

        //Raycastの座標デバッグ
        Debug.DrawLine(rightStart, rightEnd, Color.green);//後で消す
        Debug.DrawLine(leftStart, leftEnd, Color.red);//後で消す

        if (leftHit && dir == 1)
        {
            //Debug.Log("壁にあたった：左");
            dir = -1;
            if (Sp) Sp.flipX = true;
        }
        //壁に接触したか判断する
        else if (rightHit && dir == -1)
        {
            //Debug.Log("壁にあたった：右");
            dir = 1;
            if (Sp) Sp.flipX = false;
        }
    }

    //プレイヤーを倒す
    public virtual void Defeat_Player()
    {
        
    }

    //自分（敵）がやられる
    public virtual void Defeat_Enemy()
    {

    }

    public virtual void Reset_process()
    {
        //Debug.Log("働くやでー");
        transform.position = FirstPos;
        Spawned = false;
        dir = 1;
        if(Sp)Sp.flipX = false;

        gameObject.SetActive(true);
    }

    protected GameObject ParentObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    protected GameObject ChildObject(int num)
    {
        return gameObject.transform.GetChild(num).gameObject;
    }
}
