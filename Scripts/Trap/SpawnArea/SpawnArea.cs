using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : TrapManager
{
    public GameObject SpawnObject;
    public float Speed;
    private GameObject SpawnedObject;
    //private Tween _tween = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (TrapPlayed) //��������̑��x�ȏ�Œ@���΍쓮
            {
                return;
            }

            //Debug.Log("�G�o���I�I�I");

            TrapPlayed = true;//�쓮����

            SpawnedObject = Instantiate(SpawnObject, this.transform.position, Quaternion.identity);

            //if (Speed != 0) _tween = SpawnedObject.transform.DOMoveX(this.transform.position.x + Speed, 0.5f);
        }
    }

    public override void Trap_Reset()
    {
        base.Trap_Reset();
        if(SpawnedObject) Destroy(SpawnedObject);
    }
}
