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
    [Range(1f, 24f)] public float speed = 0.08f;//速度

    public RectTransform PressAnyKey_tx;
    public RectTransform Gamestart_tx;
    public RectTransform Option_tx;

    public GameObject PlayerObj;
    public GameObject EnemyObj;

    private bool Text_anim;
    private bool Selected;//falseならば、GameStartを選んでいる

    [HideInInspector]public bool Title_End = true;
    [HideInInspector] public bool Effect_End;

    private bool GroundStop;//地面を止める

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
            Title_End = true;//タイトル画面ではもう何もできなくする

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
        //if (!Alive) return;//既にやられている場合は実行しない

        //Alive = false;//死ぬ

        PlayerObj.GetComponent<Animator>().SetBool("Defeat", true);

        //効果音を鳴らす
        AudioManager.Instance.Play_SE(0);

        //子オブジェクトのコライダーの判定もいったん消す
        //gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //数秒止まって
        //rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //プレイヤーのスプライトを最前面に
        PlayerObj.GetComponent<SpriteRenderer>().sortingOrder = 100;

        //高く飛び上がり、そのまま落下
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

        //カメラの下で数秒待機
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
            -1,//完了時の値
            1 / speed//完了までの時間
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

    //オープンアニメーション
    public void OpenAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        float myInt = 0;
        DOTween.To(
            () => myInt,
            x => myInt = x,
            1,//完了時の値
            0.5f//完了までの時間
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

    //クローズアニメーション
    public void CloseAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));

        float myInt = 1;
        DOTween.To(
            () => myInt,
            x => myInt = x,
            0,//完了時の値
            1f//完了までの時間
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
