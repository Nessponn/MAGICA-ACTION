using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

    protected bool TrapPlayed;//�g���b�v���쓮������A������B
    protected Vector3 Firstpos;

    public virtual void Start()
    {
        Firstpos = gameObject.transform.position;
    }

    protected GameObject ParentObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    //�g���b�v�t�H���_�ւ̓o�^
    protected void ResisterFold(GameObject obj)
    {
        obj.transform.parent = GameMaster.Instance.TrapFolder.transform;
    }

    //�v���C���[�����ꂽ�Ƃ��ɌĂяo��
    public virtual void Trap_Reset()
    {
        //�����ɂ͉��������Ȃ�

        TrapPlayed = false;
    }
}
