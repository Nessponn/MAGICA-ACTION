using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーへの当たり判定に関する処理
public class HitBox_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //敵に対する当たり判定

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if(layername == "Enemy")
        {
            //死にます！！！

            Debug.Log("死んだ！！１");
        }
    }
}
