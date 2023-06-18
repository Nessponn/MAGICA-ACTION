using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [Range(0, 1f)] public float speed = 0.5f;//���x
    public LayerMask floorLayer;  //�����C���[
    public LayerMask wallLayer;   //�ǃ��C���[

    protected Rigidbody2D rbody;
    protected RaycastHit2D hit;//���n����
    protected RaycastHit2D leftHit;//���̕ǂ̐ڐG����
    protected RaycastHit2D rightHit;//�E�̕ǂ̐ڐG����

    protected Vector2 FirstPos;//�����ʒu
    protected SpriteRenderer Sp;
    protected bool Landing;//���n���Ă��邩
    [HideInInspector] public int dir = 1;//�i��ł�������B1�Ȃ獶�A-1�Ȃ�E

    [HideInInspector]public bool Spawned;//�o��������

    // Start is called before the first frame update
    public virtual void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        //�n�_�ƏI�_�iRay�`�ʗp�B���ۂ̔���ł͂Ȃ��j
        Vector3 origin = transform.position;

        //���ۂ�Ray�̔���
        hit = Physics2D.Raycast(origin, Vector2.down, 0.8f, floorLayer);

        Spawned = true;//�o������

        Sp = GetComponent<SpriteRenderer>();

        gameObject.SetActive(true);
    }

    public virtual void ResisterPosition()
    {
        FirstPos = gameObject.transform.position;//�����ʒu�̓o�^
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //rbody.velocity = new Vector2(-0.5f, rbody.velocity.y);

        if(rbody)Landing_judgment();//�n�ʂ̔���

        wall_judgment();//�ǂ̔���
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    void Landing_judgment()
    {
        //���ۂ�Ray�̔���
        Vector3 origin = transform.position;//�n�_
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
        //�E
        Vector2 rightStart = transform.position + (transform.right * 0.55f) - (transform.up * 0.2f);
        Vector2 rightEnd = transform.position + (transform.right * 0.55f) - (transform.up * 0.1f);
        //��
        Vector2 leftStart = transform.position - (transform.right * 0.55f) - (transform.up * 0.2f);
        Vector2 leftEnd = transform.position - (transform.right * 0.55f) - (transform.up * 0.1f);
        //Raycast�𒣂�
        RaycastHit2D leftHit = Physics2D.Linecast(leftStart, leftEnd, wallLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);

        //Raycast�̍��W�f�o�b�O
        Debug.DrawLine(rightStart, rightEnd, Color.green);//��ŏ���
        Debug.DrawLine(leftStart, leftEnd, Color.red);//��ŏ���

        if (leftHit && dir == 1)
        {
            //Debug.Log("�ǂɂ��������F��");
            dir = -1;
            if (Sp) Sp.flipX = true;
        }
        //�ǂɐڐG���������f����
        else if (rightHit && dir == -1)
        {
            //Debug.Log("�ǂɂ��������F�E");
            dir = 1;
            if (Sp) Sp.flipX = false;
        }
    }

    //�v���C���[��|��
    public virtual void Defeat_Player()
    {
        
    }

    //�����i�G�j�������
    public virtual void Defeat_Enemy()
    {

    }

    public virtual void Reset_process()
    {
        //Debug.Log("������Ł[");
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
