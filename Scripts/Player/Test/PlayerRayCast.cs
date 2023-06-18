using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public LayerMask floorLayer;  //床レイヤー
    public LayerMask wallLayer;   //壁レイヤー

    private Rigidbody2D rbody;      //球のRigidbody
    //private float moveSpeed = 0.0f;  //球の移動速度

    [Range(0, 0.15f)] public float speed = 0.08f;

    GameObject cam;



    private float MAXSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //床用のLinecast座標
        Vector2 underStart = transform.position - (transform.right * 0.5f) - (transform.up * 0.51f);
        Vector2 underEnd = transform.position + (transform.right * 0.5f) - (transform.up * 0.51f);

        //壁用のLinecast座標
        Vector2 rightStart = transform.position + (transform.right * 0.51f) + (transform.up * 0.5f);
        Vector2 rightEnd = transform.position + (transform.right * 0.51f) - (transform.up * 0.5f);

        //Linecastを張る
        RaycastHit2D underHit = Physics2D.Linecast(underStart, underEnd, floorLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);

        //床に接触してるか判断する
        /*if (underHit) moveSpeed = 1f;
        else moveSpeed =0f;*/

        if (underHit)
        {

        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!(rbody.velocity.x <= -MAXSpeed)) rbody.velocity += new Vector2(-speed, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!(rbody.velocity.x >= MAXSpeed)) rbody.velocity += new Vector2(speed, 0);
        }



        //壁に接触したか判断する
        if (rightHit) Debug.Log("動けない");

        //Linecastの座標デバッグ
        Debug.DrawLine(underStart, underEnd, Color.blue);
        Debug.DrawLine(rightStart, rightEnd, Color.green);


        cam.transform.position = new Vector3(gameObject.transform.position.x, 0, -10);
    }

    void FixedUpdate()
    {
        //rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
    }



    private void Move()
    {

    }
}
