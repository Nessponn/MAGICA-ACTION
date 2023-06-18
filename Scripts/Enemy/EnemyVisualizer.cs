using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualizer : MonoBehaviour
{
    //カメラに居ない時に敵を非表示に
    //カメラに映る範囲内に来たら、表示する

    CameraWatcher CamWatcher;
    private GameObject Cam;//カメラの現在位置
    private bool Watching;//監視中かどうか
    private Vector2 WatchDistance;//監視距離

    void Start()
    {
        Cam = Camera.main.gameObject;//カメラ
        WatchDistance = GameMaster.Instance.WatchDistance;//監視距離

        //常時カメラを監視する親オブジェクトを作成
        GameObject Cam_Watchman = Instantiate(new GameObject(),Vector2.zero, Quaternion.identity);

        //カメラ監視役を敵フォルダの子に配置
        Cam_Watchman.transform.parent = GameMaster.Instance.EnemyFolder.transform;

        //敵のオブジェクトをカメラ監視オブジェクトの子オブジェクトとして登録
        gameObject.transform.parent = Cam_Watchman.transform;

        //監視役に、監視用のスクリプトをセット＆紐づけ
        CamWatcher =  Cam_Watchman.AddComponent<CameraWatcher>();
        CamWatcher.ResisterEnemy(gameObject);
        CamWatcher.StartWatching();//監視開始

        //オブジェクトを非表示
        EnemySleep();
    }

    private void Update()
    {
        //Debug.Log("x =" + (Cam.transform.position.x - gameObject.transform.position.x));
        //Debug.Log("y =" + (Cam.transform.position.y - gameObject.transform.position.y));
        //監視範囲外にカメラが出たら、敵を消す
        if (Mathf.Abs(Cam.transform.position.x - gameObject.transform.position.x) > WatchDistance.x
            || Mathf.Abs(Cam.transform.position.y - gameObject.transform.position.y) > WatchDistance.y)
        {
            EnemySleep();//監視を中断する
        }
    }

    //カメラに入ったら、敵を表示する
    public void EnemyWakeUp()
    {
        //ここは別スクリプト（名前未定）で処理する
    }

    //カメラからでたら、敵を引っ込める
    public void EnemySleep()
    {
        CamWatcher.StartWatching();//監視を再開
        gameObject.SetActive(false);
    }
}
