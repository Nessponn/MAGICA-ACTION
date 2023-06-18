using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatcher : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject EnemyObject;//出現させる敵オブジェクト対象

    private GameObject Cam;//カメラの現在位置
    private bool Watching;//監視中かどうか
    private Vector2 WatchDistance;//監視距離
    private bool Dist;
    private Vector2 FirstPos;//敵の初期位置

    private void Start()
    {
        Cam = Camera.main.gameObject;
        WatchDistance = GameMaster.Instance.WatchDistance;
        FirstPos = gameObject.transform.GetChild(0).gameObject.transform.position;
    }

    //カメラの位置の監視を行う
    private void Update()
    {
        if (EnemyObject == null) return;

        if (!Watching) return;
        

        //監視範囲内にカメラが入ってきたら、敵を出現させる
        if (Mathf.Abs(Cam.transform.position.x - EnemyObject.transform.position.x) <= WatchDistance.x
            && Mathf.Abs(Cam.transform.position.y - EnemyObject.transform.position.y) <= WatchDistance.y && !Dist)
        {
            StopWatching();//監視を中断する
        }

        //カメラが初期位置に入ってきたら、敵を出現させる
        if (Mathf.Abs(Cam.transform.position.x - FirstPos.x) < WatchDistance.x - 0.1f
            && Mathf.Abs(Cam.transform.position.y - FirstPos.y) < WatchDistance.y - 0.1f)
        {
            //StopWatching();//監視を中断する
            //Debug.Log("あああああああああああああああああああああああああああああ");

            Dist = true;
        }
        else
        {
            Dist = false;
        }
    }

    //監視開始
    public void StartWatching()
    {
        Watching = true;
    }

    //監視終了
    public void StopWatching()
    {
        Watching = false;
        EnemyObject.SetActive(true);
    }

    //操作対象の敵を登録
    public void ResisterEnemy(GameObject enemy)
    {
        EnemyObject = enemy;
        FirstPos = enemy.transform.position;

        //Debug.Log("FirstPos = "+ FirstPos);
    }
}
