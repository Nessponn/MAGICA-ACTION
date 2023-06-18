using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualizer : MonoBehaviour
{
    //�J�����ɋ��Ȃ����ɓG���\����
    //�J�����ɉf��͈͓��ɗ�����A�\������

    CameraWatcher CamWatcher;
    private GameObject Cam;//�J�����̌��݈ʒu
    private bool Watching;//�Ď������ǂ���
    private Vector2 WatchDistance;//�Ď�����

    void Start()
    {
        Cam = Camera.main.gameObject;//�J����
        WatchDistance = GameMaster.Instance.WatchDistance;//�Ď�����

        //�펞�J�������Ď�����e�I�u�W�F�N�g���쐬
        GameObject Cam_Watchman = Instantiate(new GameObject(),Vector2.zero, Quaternion.identity);

        //�J�����Ď�����G�t�H���_�̎q�ɔz�u
        Cam_Watchman.transform.parent = GameMaster.Instance.EnemyFolder.transform;

        //�G�̃I�u�W�F�N�g���J�����Ď��I�u�W�F�N�g�̎q�I�u�W�F�N�g�Ƃ��ēo�^
        gameObject.transform.parent = Cam_Watchman.transform;

        //�Ď����ɁA�Ď��p�̃X�N���v�g���Z�b�g���R�Â�
        CamWatcher =  Cam_Watchman.AddComponent<CameraWatcher>();
        CamWatcher.ResisterEnemy(gameObject);
        CamWatcher.StartWatching();//�Ď��J�n

        //�I�u�W�F�N�g���\��
        EnemySleep();
    }

    private void Update()
    {
        //Debug.Log("x =" + (Cam.transform.position.x - gameObject.transform.position.x));
        //Debug.Log("y =" + (Cam.transform.position.y - gameObject.transform.position.y));
        //�Ď��͈͊O�ɃJ�������o����A�G������
        if (Mathf.Abs(Cam.transform.position.x - gameObject.transform.position.x) > WatchDistance.x
            || Mathf.Abs(Cam.transform.position.y - gameObject.transform.position.y) > WatchDistance.y)
        {
            EnemySleep();//�Ď��𒆒f����
        }
    }

    //�J�����ɓ�������A�G��\������
    public void EnemyWakeUp()
    {
        //�����͕ʃX�N���v�g�i���O����j�ŏ�������
    }

    //�J��������ł���A�G���������߂�
    public void EnemySleep()
    {
        CamWatcher.StartWatching();//�Ď����ĊJ
        gameObject.SetActive(false);
    }
}
