using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatcher : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject EnemyObject;//�o��������G�I�u�W�F�N�g�Ώ�

    private GameObject Cam;//�J�����̌��݈ʒu
    private bool Watching;//�Ď������ǂ���
    private Vector2 WatchDistance;//�Ď�����
    private bool Dist;
    private Vector2 FirstPos;//�G�̏����ʒu

    private void Start()
    {
        Cam = Camera.main.gameObject;
        WatchDistance = GameMaster.Instance.WatchDistance;
        FirstPos = gameObject.transform.GetChild(0).gameObject.transform.position;
    }

    //�J�����̈ʒu�̊Ď����s��
    private void Update()
    {
        if (EnemyObject == null) return;

        if (!Watching) return;
        

        //�Ď��͈͓��ɃJ�����������Ă�����A�G���o��������
        if (Mathf.Abs(Cam.transform.position.x - EnemyObject.transform.position.x) <= WatchDistance.x
            && Mathf.Abs(Cam.transform.position.y - EnemyObject.transform.position.y) <= WatchDistance.y && !Dist)
        {
            StopWatching();//�Ď��𒆒f����
        }

        //�J�����������ʒu�ɓ����Ă�����A�G���o��������
        if (Mathf.Abs(Cam.transform.position.x - FirstPos.x) < WatchDistance.x - 0.1f
            && Mathf.Abs(Cam.transform.position.y - FirstPos.y) < WatchDistance.y - 0.1f)
        {
            //StopWatching();//�Ď��𒆒f����
            //Debug.Log("����������������������������������������������������������");

            Dist = true;
        }
        else
        {
            Dist = false;
        }
    }

    //�Ď��J�n
    public void StartWatching()
    {
        Watching = true;
    }

    //�Ď��I��
    public void StopWatching()
    {
        Watching = false;
        EnemyObject.SetActive(true);
    }

    //����Ώۂ̓G��o�^
    public void ResisterEnemy(GameObject enemy)
    {
        EnemyObject = enemy;
        FirstPos = enemy.transform.position;

        //Debug.Log("FirstPos = "+ FirstPos);
    }
}
