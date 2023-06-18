using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameMaster : SingletonMonoBehaviourFast<GameMaster>
{
    //Unity��Hielarchy�K�͂ŃI�u�W�F�N�g�̑�����s���B

    public GameObject EnemyFolder;//�G�̃t�H���_�[
    public GameObject TrapFolder;//�g���b�v�̃t�H���_�[
    public GameObject ObjectFolder;//�I�u�W�F�N�g�̃t�H���_�[

    public Vector2 WatchDistance;//�G�̏o������
    public Vector2 CameraScroll_min;//�J�����X�N���[���̍��[
    public Vector2 CameraScroll_max;//�J�����X�N���[���̉E�[
    public SpriteMask Mask;
    public TextMeshProUGUI RestTx;//�c�@���̃e�L�X�g

    private static int Rest = 2;//�c�@��
    private GameObject cam;//�J����
    private bool camLock;//�J�������b�N����
    private GameObject Player;//�v���C���[�̈ʒu
    private bool Cleared;//�N���A������true

    public GameObject EffectWhite;
    public GameObject EffectWhite_Small;
    public GameObject EffectYellow;
    public GameObject EffectYellow_Small;

    public bool requestscene;
    // Start is called before the first frame update
    void Start()
    {
        Game_Start();//�Q�[���J�n����
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
            //�ŏ��̃X�e�[�W�X�^�[�g����
            OpenAnimation();
            AudioManager.Instance.Play_BGM(0);
        }).SetLink(gameObject);

        cam.transform.position = Player.transform.position;

        RestTx.text = "MAGICA x " + Rest;

        //�G�̏����ʒu��o�^
        for (int i = 0; i < EnemyFolder.transform.childCount; i++)
        {
            GameObject obj = EnemyFolder.transform.GetChild(i).gameObject;
            EnemyManager eneCop = obj.GetComponent<EnemyManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (eneCop) break;
                //Debug.Log("����Ă�");

                var Cop = obj.transform.GetChild(j).GetComponent<EnemyManager>();

                if (Cop)
                {
                    eneCop = Cop;
                    //Debug.Log("�擾");
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
            //Debug.Log("�ŏ�");
        }

        if (CameraScroll_max.x < cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
            //Debug.Log("�ő�");
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
            Debug.Log("�ŏ�");
        }
        else if (CameraScroll_max.x < cam.transform.position.x)
        {
            cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
            Debug.Log("�ő�");
            if (CameraScroll_max.x <= cam.transform.position.x)
            {
                cam.transform.position = new Vector3(Player.transform.position.x, 0, -10);
            }
        }*/
        /*
                if (CameraScroll_min.x > cam.transform.position.x)
                {
                    cam.transform.position = new Vector3(CameraScroll_min.x, 0, -10);
                    Debug.Log("�ő�");
                }
                else if (CameraScroll_max.x < cam.transform.position.x)
                {
                    cam.transform.position = new Vector3(CameraScroll_max.x, 0, -10);
                    Debug.Log("�ŏ�");
                }
                else
                {

                }*/


        //���������玀��
        if (cam.transform.position.y + Player.transform.position.y < -8) PlayerManager_Ray.Instance.Fall_process();
    }

    public void ClearFlag()
    {
        Cleared = true;
        AudioManager.Instance.Stop_BGM();
    }

    //�N���A��Ԃ�����
    public void NotClearFlag()
    {
        Cleared = false;
    }

    //�c�@�����炷
    public void RestDecrease()
    {
        Rest--;

        RestTx.text = "MAGICA x " + Rest;

    }

    //�ăX�^�[�g����
    public void Restart_process()
    {
        //�J�ڃA�j���[�V����
        //�g���b�v�̏�Ԃ����Z�b�g����
        for (int i = 0; i < TrapFolder.transform.childCount; i++)
        {
            GameObject obj = TrapFolder.transform.GetChild(i).gameObject;
            TrapManager TrapCop = obj.GetComponent<TrapManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (TrapCop) break;
                //Debug.Log("����Ă�");

                var Cop = obj.transform.GetChild(j).GetComponent<TrapManager>();

                if (Cop)
                {
                    TrapCop = Cop;
                    //Debug.Log("�擾");
                };
            }

            if (TrapCop)
            {
                TrapCop.Trap_Reset();
            }
        }
        //�G�̏�ԃ��Z�b�g
        for (int i = 0; i < EnemyFolder.transform.childCount; i++)
        {
            GameObject obj = EnemyFolder.transform.GetChild(i).gameObject;
            EnemyManager eneCop = obj.GetComponent<EnemyManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (eneCop) break;
                //Debug.Log("����Ă�");

                var Cop = obj.transform.GetChild(j).GetComponent<EnemyManager>();

                if (Cop)
                {
                    eneCop = Cop;
                    //Debug.Log("�擾");
                };
            }

            if (eneCop)
            {
                eneCop.Reset_process();
            }
        }

        

        //�I�u�W�F�N�g�̏�Ԃ����Z�b�g
        for (int i = 0; i < ObjectFolder.transform.childCount; i++)
        {
            GameObject obj = ObjectFolder.transform.GetChild(i).gameObject;
            ObjectManager ObjCop = obj.GetComponent<ObjectManager>();

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (ObjCop) break;
                //Debug.Log("����Ă�");

                var Cop = obj.transform.GetChild(j).GetComponent<ObjectManager>();

                if (Cop)
                {
                    ObjCop = Cop;
                    //Debug.Log("�擾");
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
            })
            .SetLink(Mask.gameObject);
    }

    //�N���[�Y�A�j���[�V����
    public void CloseAnimation()
    {
        Mask.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));

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
            })
            .SetLink(Mask.gameObject);
    }
}
