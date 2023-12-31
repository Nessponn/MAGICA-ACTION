using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //敵に対する当たり判定
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //壁に接触
            PlayerManager.Instance.Wall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //敵に対する当たり判定
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //壁から離れる
            PlayerManager.Instance.Wall = false;
        }
    }
}
