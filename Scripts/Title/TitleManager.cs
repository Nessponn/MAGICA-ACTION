using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class TitleManager : SingletonMonoBehaviourFast<TitleManager>
{
    public SpriteMask Mask;
    public GameObject Ground;
    public GameObject Letter;
    [Range(1f, 24f)] public float speed = 0.08f;//���x

    public RectTransform PressAnyKey_tx;
    public RectTransform Gamestart_tx;
    public RectTransform Option_tx;

    public GameObject PlayerObj;
    public GameObject EnemyObj;

    private bool Text_anim;
    private bool Selected;//false�Ȃ�΁AGameStart��I��ł���

    [HideInInspector]public bool Title_End = true;
    [HideInInspector] public bool Effect_End;

    private bool GroundStop;//�n�ʂ��~�߂�

    public GameObject EffectWhite;
    public GameObject EffectWhite_Small;
    public GameObject EffectYellow;
    public GameObject EffectYellow_Small;

    public string request_scenename;


    // Start is called before the first frame update
    void Start()
    {
        Mask.alphaCutoff = 0;

        GroundMove();

        Letter.transform.DOMoveX(0, 2f);

        DOVirtual.DelayedCall(4, () =>
        {
            OpenAnimation();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Title_End) return;
/*
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && Selected)
        {
            Letter.transform.DOMoveY(0, 0.05f);
            Selected = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && !Selected)
        {
            Letter.transform.DOMoveY(-1.5f, 0.05f);
            Selected = true;
        }
*/
        if(!Selected && Input.GetKeyDown(KeyCode.Space))
        {
            Title_End = true;//�^�C�g����ʂł͂��������ł��Ȃ�����

            PlayerObj.transform.DOMoveX(0,0.5f).SetLink(PlayerObj);
            EnemyObj.transform.DOMoveX(0,0.5f).SetLink(EnemyObj);
            Letter.transform.DOMoveX(20, 1f).SetLink(Letter);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                GroundStop = true;
                Effect_End = true;
                Death_process();
            });
        }
    }

    public void Death_process()
    {
        //if (!Alive) return;//���ɂ���Ă���ꍇ�͎��s���Ȃ�

        //Alive = false;//����

        PlayerObj.GetComponent<Animator>().SetBool("Defeat", true);

        //���ʉ���炷
        AudioManager.Instance.Play_SE(0);

        //�q�I�u�W�F�N�g�̃R���C�_�[�̔���������������
        //gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //���b�~�܂���
        //rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //�v���C���[�̃X�v���C�g���őO�ʂ�
        PlayerObj.GetComponent<SpriteRenderer>().sortingOrder = 100;

        //������яオ��A���̂܂ܗ���
        DOVirtual.DelayedCall(0.7f, () =>
        {
            //rbody.constraints = RigidbodyConstraints2D.None;
            //rbody.constraints = RigidbodyConstraints2D.FreezePositionX;

            //rbody.velocity = new Vector2(0, 22);

            PlayerObj.transform.DOJump(new Vector2(0, -20), 20, 1,3).SetLink(PlayerObj);

        });

        DOVirtual.DelayedCall(1f, () =>
        {
            CloseAnimation();
        });

        //�J�����̉��Ő��b�ҋ@
        DOVirtual.DelayedCall(3f, () =>
        {
            //rbody.constraints = RigidbodyConstraints2D.FreezeAll;
            SceneManager.LoadScene(request_scenename);
        });

    }


    public void LetterAnim()
    {
        Letter.transform.DOMoveX(1.5f, 0.3f);

        Title_End = false;
    }

    public void TextAnimation()
    {
        Text_anim = !Text_anim;

        if (Text_anim)
        {
            Gamestart_tx.DOAnchorPosX(180, 0.3f);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                Option_tx.DOAnchorPosX(180, 0.3f);
            });
        }
        else
        {
            Gamestart_tx.DOAnchorPosX(800, 0.3f);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                Option_tx.DOAnchorPosX(800, 0.3f);
            });
        }
        
    }

    public void GroundMove()
    {
        float myInt = 0;
        DOTween.To(
            () => myInt,
            x => myInt = x,
            -1,//�������̒l
            1 / speed//�����܂ł̎���
            )
            .OnUpdate(() =>
            {
                Ground.transform.position = new Vector2(myInt,0); 
            })
            .OnComplete(() =>
            {
                if(!GroundStop)GroundMove();
            }).SetEase(Ease.Linear);
    }

    //�I�[�v���A�j���[�V����
    public void OpenAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        float myInt = 0;
        DOTween.To(
            () => myInt,
            x => myInt = x,
            1,//�������̒l
            0.5f//�����܂ł̎���
            )
            .OnUpdate(() =>
            {
                Mask.alphaCutoff = myInt;
            })
            .OnComplete(() =>
            {
                Mask.alphaCutoff = 1;
            });
    }

    //�N���[�Y�A�j���[�V����
    public void CloseAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));

        float myInt = 1;
        DOTween.To(
            () => myInt,
            x => myInt = x,
            0,//�������̒l
            1f//�����܂ł̎���
            )
            .OnUpdate(() =>
            {
                Mask.alphaCutoff = myInt;
            })
            .OnComplete(() =>
            {
                Mask.alphaCutoff = 0;
            });
    }
}
