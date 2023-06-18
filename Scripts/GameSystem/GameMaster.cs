using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameMaster : SingletonMonoBehaviourFast<GameMaster>
{
    //UnityのHielarchy規模でオブジェクトの操作を行う。

    public GameObject EnemyFolder;//敵のフォルダー
    public GameObject TrapFolder;//トラップのフォルダー
    public GameObject ObjectFolder;//オブジェクトのフォルダー

    public Vector2 WatchDistance;//敵の出現距離
    public Vector2 CameraScroll_min;//カメラスクロールの左端
    public Vector2 CameraScroll_max;//カメラスクロールの右端
    public SpriteMask Mask;
    public TextMeshProUGUI RestTx;//残機数のテキスト

    private static int Rest = 2;//残機数
    private GameObject cam;//カメラ
    private bool camLock;//カメラロック時間
    private GameObject Player;//プレイヤーの位置
    private bool Cleared;//クリアしたらtrue

    public GameObject EffectWhite;
    public GameObject EffectWhite_Small;
    public GameObject EffectYellow;
    public GameObject EffectYellow_Small;

    public bool requestscene;
    // Start is called before the first frame update
    void Start()
    {
        Game_Start();//ゲーム開始処理
    }

    public int GetRest()
    {
        return Rest;
    }

    void Game_Start()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 800, sequencesCapacity: 200);

        Mask.alphaCutoff = 0;
        cam = Camera.main.gameObject;

        Player = PlayerManager_Ray.Instance.gameObject;

        DOVirtual.DelayedCall(1.5f, () =>
        {
            //最初のステージスタート処理
            OpenAnimation();
            AudioManager.Instance.Play_BGM(0);
        }).SetLink(gameObject);

        cam.transform.position = Player.transform.position;

        RestTx.text = "MAGICA x " + Rest;

        //敵の初期位置を登録
        for (int i = 0; i < EnemyFolder.transform.childCount; i++)
        {
            GameObject obj = EnemyFolder.transform.GetChild(i).gameObject;
            EnemyManager eneCop = obj.GetComponent<EnemyManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (eneCop) break;
                //Debug.Log("回ってる");

                var Cop = obj.transform.GetChild(j).GetComponent<EnemyManager>();

                if (Cop)
                {
                    eneCop = Cop;
                    //Debug.Log("取得");
                };
            }

            if (eneCop)
            {
                eneCop.ResisterPosition();
            }
        }
    }

    public void CamLock(bool b)
    {
        camLock = b;
    }

    private void Update()
    {
        if (Cleared || camLock) return;

        cam.transform.position = new Vector3(Player.transform.position.x, 0, -10);

        if (CameraScroll_min.x > cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_min.x, 0, -10);
            //Debug.Log("最小");
        }

        if (CameraScroll_max.x < cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
            //Debug.Log("最大");
        }

        if(Player.transform.position.x - CameraScroll_min.x <= -9)
        {
            Player.transform.position = new Vector3(CameraScroll_min.x - 9, Player.transform.position.y, 0);
        }

        if (Player.transform.position.x - CameraScroll_max.x >= 9 && !requestscene)
        {
            Player.transform.position = new Vector3(CameraScroll_max.x + 9, Player.transform.position.y, 0);
        }

        /*if (!(CameraScroll_min.x >= cam.transform.position.x || CameraScroll_max.x <= cam.transform.position.x))
        {
            cam.transform.position = new Vector3(Player.transform.position.x, 0, -10);
        }
        else if (CameraScroll_min.x > cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_min.x, 0, -10);
            Debug.Log("最小");
        }
        else if (CameraScroll_max.x < cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
            Debug.Log("最大");
            if (CameraScroll_max.x <= cam.transform.position.x)
            {
                cam.transform.position = new Vector3(Player.transform.position.x, 0, -10);
            }
        }*/
        /*
                if (CameraScroll_min.x > cam.transform.position.x)
                {
                    cam.transform.position = new Vector3(CameraScroll_min.x, 0, -10);
                    Debug.Log("最大");
                }
                else if (CameraScroll_max.x < cam.transform.position.x)
                {
                    cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
                    Debug.Log("最小");
                }
                else
                {

                }*/


        //落下したら死ぬ
        if (cam.transform.position.y + Player.transform.position.y < -8) PlayerManager_Ray.Instance.Fall_process();
    }

    public void ClearFlag()
    {
        Cleared = true;
        AudioManager.Instance.Stop_BGM();
    }

    //クリア状態を解除
    public void NotClearFlag()
    {
        Cleared = false;
    }

    //残機を減らす
    public void RestDecrease()
    {
        Rest--;

        RestTx.text = "MAGICA x " + Rest;

    }

    //再スタート処理
    public void Restart_process()
    {
        //遷移アニメーション
        //トラップの状態をリセットする
        for (int i = 0; i < TrapFolder.transform.childCount; i++)
        {
            GameObject obj = TrapFolder.transform.GetChild(i).gameObject;
            TrapManager TrapCop = obj.GetComponent<TrapManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (TrapCop) break;
                //Debug.Log("回ってる");

                var Cop = obj.transform.GetChild(j).GetComponent<TrapManager>();

                if (Cop)
                {
                    TrapCop = Cop;
                    //Debug.Log("取得");
                };
            }

            if (TrapCop)
            {
                TrapCop.Trap_Reset();
            }
        }
        //敵の状態リセット
        for (int i = 0; i < EnemyFolder.transform.childCount; i++)
        {
            GameObject obj = EnemyFolder.transform.GetChild(i).gameObject;
            EnemyManager eneCop = obj.GetComponent<EnemyManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (eneCop) break;
                //Debug.Log("回ってる");

                var Cop = obj.transform.GetChild(j).GetComponent<EnemyManager>();

                if (Cop)
                {
                    eneCop = Cop;
                    //Debug.Log("取得");
                };
            }

            if (eneCop)
            {
                eneCop.Reset_process();
            }
        }

        

        //オブジェクトの状態をリセット
        for (int i = 0; i < ObjectFolder.transform.childCount; i++)
        {
            GameObject obj = ObjectFolder.transform.GetChild(i).gameObject;
            ObjectManager ObjCop = obj.GetComponent<ObjectManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (ObjCop) break;
                //Debug.Log("回ってる");

                var Cop = obj.transform.GetChild(j).GetComponent<ObjectManager>();

                if (Cop)
                {
                    ObjCop = Cop;
                    //Debug.Log("取得");
                };
            }

            if (ObjCop)
            {
                ObjCop.Object_Reset();
            }
        }

        camLock = false;

        AudioManager.Instance.Play_BGM(0);
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
            })
            .SetLink(Mask.gameObject);
    }

    //クローズアニメーション
    public void CloseAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));

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
            })
            .SetLink(Mask.gameObject);
    }
}
