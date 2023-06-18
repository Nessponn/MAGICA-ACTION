using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ړ��ⓖ���蔻��̏����̑������s��
public class PlayerManager : SingletonMonoBehaviourFast<PlayerManager>
{
    public Rigidbody2D rbody;

    [Range(0, 0.15f)] public float speed = 0.08f;


    private float MAXSpeed = 7f;

    GameObject cam;

    [HideInInspector]public bool Jump;

    [HideInInspector] public bool Wall ;

    float currentSpeed;

    float RockTime;//�d������


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
            if (rbody.velocity.x <= -MAXSpeed)//���ɉ������߂����ꍇ�A����
            {
                rbody.velocity += new Vector2(speed, 0);
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    //���ɐi�s����

                    MoveLeft();
                }
            }

            if (rbody.velocity.x >= MAXSpeed)//�E�ɉ������߂����ꍇ�A����
            {
                rbody.velocity += new Vector2(-speed, 0);
            }
            else//Max�X�s�[�h�ȉ��ł���΁A������󂯕t����
            {
                if (Input.GetKey(KeyCode.D))
                {
                    //�E�ɐi�s����

                    MoveRight();
                }
            }

            /*if (Input.GetKey(KeyCode.A))
            {
                //���ɐi�s����

                if (!(rbody.velocity.x <= -7)) MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //�E�ɐi�s����

                if (!(rbody.velocity.x >= 7)) MoveRight();
            }*/

            if (Input.GetKey(KeyCode.Space) && Jump)
            {
                JumpUp();//�W�����v�{�^��������
            }
            else if (rbody.velocity.y >= 3 && Input.GetKeyUp(KeyCode.Space))
            {
                JumpDown();//�W�����v���Ƀ{�^���𗣂�
            }

            /*//�n�ʂɂ���Ƃ��A�n�ʂɋz���t���悤���邱�ƂŁA�����Ă���r���ɕ����Ă��܂����ۂ�h�~
            if (Jump)
            {
                if (rbody.velocity.y > 0f)
                {
                    Debug.Log("�����ȁI");
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

        //���x��ۑ�
        currentSpeed = rbody.velocity.x;

    }

    private void MoveLeft()
    {
        //���ɓ���
        if(Jump)rbody.velocity += new Vector2(-speed, 0);//�n��
        else rbody.velocity += new Vector2(-speed/2, 0);//��

        //Tilemap�̔���ɓ˂�������s��ւ̑Ώ�
        if (Mathf.Abs(rbody.velocity.x) <= 0.09f && currentSpeed < -0.07f && !Wall)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y +0.01f);
            rbody.velocity = new Vector2(currentSpeed, rbody.velocity.y);
            Debug.Log("�˂���������");
        }
    }

    private void MoveRight()
    {
        //�E�ɓ���
        if (Jump) rbody.velocity += new Vector2(speed, 0);
        else rbody.velocity += new Vector2(speed/2, 0);

        //Tilemap�̔���ɓ˂�������s��ւ̑Ώ�
        if (Mathf.Abs(rbody.velocity.x) <= 0.09f && currentSpeed > 0.07f && !Wall)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
            rbody.velocity = new Vector2(currentSpeed, rbody.velocity.y);
            Debug.Log("�˂���������");
        }
    }

    //�W�����v���̏���
    public void JumpUp()
    {
        PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.None;
        PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Jump = false;

        rbody.velocity = new Vector2(rbody.velocity.x, 20);
    }
    //�W�����v���ɃW�����v�{�^���𗣂�
    public void JumpDown()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, 3);
    }

    //���n���̏���
    public void Ground()
    {
        rbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        Jump = true;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.015f);

        if (Mathf.Abs(rbody.velocity.x) > MAXSpeed - 4)
        {
            //Debug.Log("�����I");
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
