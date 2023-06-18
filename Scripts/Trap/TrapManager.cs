using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

    protected bool TrapPlayed;//トラップが作動したら、これを。
    protected Vector3 Firstpos;

    public virtual void Start()
    {
        Firstpos = gameObject.transform.position;
    }

    protected GameObject ParentObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    //トラップフォルダへの登録
    protected void ResisterFold(GameObject obj)
    {
        obj.transform.parent = GameMaster.Instance.TrapFolder.transform;
    }

    //プレイヤーがやられたときに呼び出し
    public virtual void Trap_Reset()
    {
        //ここには何も書かない

        TrapPlayed = false;
    }
}
