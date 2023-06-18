using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public LayerMask floorLayer;  //�����C���[
    public LayerMask wallLayer;   //�ǃ��C���[

    private Rigidbody2D rbody;      //����Rigidbody
    //private float moveSpeed = 0.0f;  //���̈ړ����x

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
        //���p��Linecast���W
        Vector2 underStart = transform.position - (transform.right * 0.5f) - (transform.up * 0.51f);
        Vector2 underEnd = transform.position + (transform.right * 0.5f) - (transform.up * 0.51f);

        //�Ǘp��Linecast���W
        Vector2 rightStart = transform.position + (transform.right * 0.51f) + (transform.up * 0.5f);
        Vector2 rightEnd = transform.position + (transform.right * 0.51f) - (transform.up * 0.5f);

        //Linecast�𒣂�
        RaycastHit2D underHit = Physics2D.Linecast(underStart, underEnd, floorLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);

        //���ɐڐG���Ă邩���f����
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



        //�ǂɐڐG���������f����
        if (rightHit) Debug.Log("�����Ȃ�");

        //Linecast�̍��W�f�o�b�O
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
