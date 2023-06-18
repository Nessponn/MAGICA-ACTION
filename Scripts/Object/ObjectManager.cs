using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    protected bool ObjectPlayed;//�쓮�����I�u�W�F�N�g�Ȃ�true
    protected Vector3 Firstpos;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Firstpos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected GameObject ParentObject()
    {
        return gameObject.transform.parent.gameObject;
    }

    public virtual void Gimmick_Start()
    {

    }

    public virtual void Object_Reset()
    {
        //�����ɂ͉��������Ȃ�

        ObjectPlayed = false;


        gameObject.SetActive(true);
    }

    public virtual void ResisterPosition(Vector2 vec)
    {

    }
}
