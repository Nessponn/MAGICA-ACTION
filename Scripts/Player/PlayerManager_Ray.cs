using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager_Ray : SingletonMonoBehaviourFast<PlayerManager_Ray>
{
    [Range(0, 1f)] public float speed = 0.08f;//���x
    [Range(0.01f, 1f)] public float Friction = 0.05f;//�����쎞�̑��x�����W��
    [Range(7, 15)] public float MAXSpeed = 8f;//�ő呬�x
    [Range(1f, 2f)] public float Landing_acceleration = 1.2f;//���n���̉����x
    [Range(15f, 40f)] public float JumpPower =25f;//�W�����v��

    [HideInInspector] public Rigidbody2D rbody;//Rigidbody
    [HideInInspector] public Vector2 FirstPos;//�����ʒu�A���Ԓn�_
    [HideInInspector] public bool Cleared;//�N���A������true
    [HideInInspector] public SpriteRenderer Sp;//�X�v���C�g�R���|�[�l���g
    public LayerMask floorLayer;  //�����C���[
    public LayerMask wallLayer;   //�ǃ��C���[
    public LayerMask enemyLayer;  //�G���C���[
    public LayerMask ladderLayer; //�͂����A���̖؃��C���[

    private float RockTime;//�d������
    private bool Landing;//���n���Ă��邩
    private bool Alive;//�������Ă��邩
    private bool Ladder;//��q�A���̖؂ɕ߂܂��Ă��邩
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
        //Debug.Log("�ȂȂȂȂ�");

        rbody.constraints = RigidbodyConstraints2D.None;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        //����𕜊�������
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
        transform.position = FirstPos;//���Ԃɖ߂�
        Alive = true;//�����Ԃ点��

        //�Q�[���}�l�[�W���ɍăX�^�[�g�����𑗂�
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

    //�N���A���̏���
    public void ClearFlag()
    {
        Cleared = true;

        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    //�ړ�����
    void Move_processing()
    {
        //�Ǘp��Raycast
        //�E
        Vector2 rightStart = transform.position + (transform.right * 0.55f) + (transform.up * 0.4f);
        Vector2 rightEnd = transform.position + (transform.right * 0.55f) + (transform.up * -0.1f);
        //��
        Vector2 leftStart = transform.position - (transform.right * 0.55f) + (transform.up * 0.4f);
        Vector2 leftEnd = transform.position - (transform.right * 0.55f) + (transform.up * -0.1f);

        //��q�p��Raycast
        Vector2 ladderStart = transform.position - (transform.up * 0.3f);
        Vector2 ladderEnd = transform.position - (transform.up * 0.3f);

        //Raycast�𒣂�
        RaycastHit2D leftHit = Physics2D.Linecast(leftStart, leftEnd, wallLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(rightStart, rightEnd, wallLayer);
        RaycastHit2D ladderHit = Physics2D.Linecast(ladderStart, ladderEnd, ladderLayer);

        //Raycast�̍��W�f�o�b�O
        //Debug.DrawLine(rightStart, rightEnd, Color.green);//��ŏ���
        //Debug.DrawLine(leftStart, leftEnd, Color.red);//��ŏ���
        Debug.DrawLine(ladderStart, ladderEnd, Color.red);//��ŏ���

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


        if (!Ladder) Landing_processing();//�n���󒆂ɂ���Ƃ��̏���
        else Ladder_processing();//��q�ɕ߂܂��Ă���Ƃ��̏���

        //�ǂɐڐG���������f����
        if (rightHit) 
        {
            //Debug.Log("�ǂɂ��������F�E");
            if (rbody.velocity.x > 0f) rbody.velocity = new Vector2(0, rbody.velocity.y); 
        }
        if (leftHit)
        {
            //Debug.Log("�ǂɂ��������F��");
            if (rbody.velocity.x < 0f) rbody.velocity = new Vector2(0, rbody.velocity.y); 
        }

    }

    void Landing_processing()
    {
        //���ɐڐG���Ă邩���f����

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

    //���n����
    private bool EnemyStanping;

    void Landing_judgment()
    {
        //�n��
        //�n�_�ƏI�_�iRay�`�ʗp�B���ۂ̔���ł͂Ȃ��j
        Vector3 origin1 = transform.position + (transform.right * 0.35f) + (transform.up * -0.4f);
        Vector3 origin2 = transform.position - (transform.right * 0.35f) + (transform.up * -0.4f);
        Vector3 distance = Vector3.down * 0.3f;
        //���ۂ�Ray�̔���
        RaycastHit2D hit1 = Physics2D.Raycast(origin1, Vector2.down, 0.3f, floorLayer);//�n��
        RaycastHit2D hit2 = Physics2D.Raycast(origin2, Vector2.down, 0.3f, floorLayer);//�n��

        //�G
        Vector3 origin_right = transform.position + (transform.right * 0.4f) + (transform.up * -0.4f);//�E���̔���
        Vector3 origin_left = transform.position + (transform.right * -0.4f) + (transform.up * -0.4f);//�����̔���
        Vector3 distance_enemy = Vector3.down * 0.5f;
        RaycastHit2D hitEnemy1 = Physics2D.Raycast(origin_right, Vector2.down, 0.5f, enemyLayer);//�G1
        RaycastHit2D hitEnemy2 = Physics2D.Raycast(origin_left, Vector2.down, 0.5f, enemyLayer);//�G2

        //Debug.DrawRay(origin, distance, Color.yellow);
        //Debug.DrawRay(origin_right, distance_enemy, Color.yellow);
        //Debug.DrawRay(origin_left, distance_enemy, Color.yellow);

        if (hit1 && !(rbody.velocity.y > 1f))
        {
            Debug.DrawRay(origin1, distance, Color.yellow);
            transform.position = new Vector2(transform.position.x, hit1.point.y + 0.5f);

            if (!Landing && rbody.velocity.y <= -0.5f)
            {
                //���n���ɉ���
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
                rbody.velocity = new Vector2(rbody.velocity.x, 0);//������ʂ̂��̂ɕς���ƍ��o��Ȃ��Ȃ�

                //���܂ɂ��炮�炷��̂́A�̏d�Əd�͌W�����֌W���Ă��邩��
            }
        }
        else if (hit2 && !(rbody.velocity.y > 1f))
        {
            Debug.DrawRay(origin2, distance, Color.yellow);
            transform.position = new Vector2(transform.position.x, hit2.point.y + 0.5f);

            if (!Landing && rbody.velocity.y <= -0.5f)
            {
                //���n���ɉ���
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
                rbody.velocity = new Vector2(rbody.velocity.x, 0);//������ʂ̂��̂ɕς���ƍ��o��Ȃ��Ȃ�

                //���܂ɂ��炮�炷��̂́A�̏d�Əd�͌W�����֌W���Ă��邩��
            }
        }
        else
        {
            Debug.DrawRay(origin1, distance, Color.blue);
            Debug.DrawRay(origin2, distance, Color.blue);

            //���ɂ���΁A���n��Ԃ�����
            if (Landing)
            {
                Landing = false;

                anim.SetBool("Jump", false);
            }
            //���ɂ���Ƃ��ɁA�{�^���𗣂����ƂŃW�����v���L�����Z��
            if (Input.GetKeyUp(KeyCode.Space) && rbody.velocity.y > 3)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, 3);
            }

            //�d�ʊ��̂��铮����\��
            if (rbody.velocity.y > 0f)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y * 0.995f);
            }
            if (rbody.velocity.y <= -1f)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y * 0.999f);
            }
        }

        //�G�𓥂񂾎��̏���
        if ((hitEnemy1 || hitEnemy2) && !Landing)
        {
            if (!EnemyStanping)
            {
                if (hitEnemy1)
                {
                    hitEnemy1.collider.gameObject.GetComponent<EnemyManager>().Defeat_Enemy();
                    EnemyStanping = true;

                   /* //�G�Ƀ_���[�W��^����
                    if (hitEnemy1.collider.gameObject.GetComponent<EnemyManager>() && !hitEnemy2)
                    {
                        
                    }*/
                }
                else if (hitEnemy2)
                {
                    hitEnemy2.collider.gameObject.GetComponent<EnemyManager>().Defeat_Enemy();
                    EnemyStanping = true;

                    /*//�G�Ƀ_���[�W��^����
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

    //���ꂽ�Ƃ��̏���
    public void Death_process()
    {
        if (!Alive) return;//���ɂ���Ă���ꍇ�͎��s���Ȃ�

        Alive = false;//����

        anim.SetBool("Defeat", true);

        //BGM���~�߂�
        AudioManager.Instance.Stop_BGM();

        //���ʉ���炷
        AudioManager.Instance.Play_SE(0);

        //�q�I�u�W�F�N�g�̃R���C�_�[�̔���������������
        gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //���b�~�܂���
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //�c�@���炷
        GameMaster.Instance.RestDecrease();

        //�v���C���[�̃X�v���C�g���őO�ʂ�
        Sp.sortingOrder = 100;

        //������яオ��A���̂܂ܗ���
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

        //�J�����̉��Ő��b�ҋ@
        DOVirtual.DelayedCall(2.3f, () =>
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }).SetLink(gameObject); ;

        DOVirtual.DelayedCall(2.5f, () =>
        {
            //�v���C���[�̃X�v���C�g���őO�ʂ��猳�ɖ߂�
            Sp.sortingOrder = 10;

            //�v���C���[�𐶂��Ԃ点��
            ReStart();

        }).SetLink(gameObject); ;

        //GameMaster.Instance.Restart_process();
    }

    //�������̏���
    public void Fall_process()
    {
        if (!Alive) return;//���ɂ���Ă���ꍇ�͎��s���Ȃ�

        Alive = false;//����

        //BGM���~�߂�
        AudioManager.Instance.Stop_BGM();

        //�c�@���炷
        GameMaster.Instance.RestDecrease();

        //���ʉ���炷
        AudioManager.Instance.Play_SE(0);

        DOVirtual.DelayedCall(1f, () =>
        {
            GameMaster.Instance.CloseAnimation();
        }).SetLink(gameObject);

        //�J�����̉��Ő��b�ҋ@
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        DOVirtual.DelayedCall(2.5f, () =>
        {
            //Debug.Log("�ʂʂʂʂ�");
            //�v���C���[�𐶂��Ԃ点��
            ReStart();
        }).SetLink(gameObject);

        //GameMaster.Instance.Restart_process();
    }
}
