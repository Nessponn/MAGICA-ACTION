using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : MonoBehaviour
{
    //プレイヤーがブロックなどに対して頭突きをした場合の処理

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y < 1.5f) return;//下から一定の速度以上で叩けば作動
            //Debug.Log("コイン！！！！");
            AudioManager.Instance.Play_SE(5);//効果音

            PlayerManager_Ray.Instance.ForceY(1.5f, 0f);//下にちょっと勢いを付ける
        }
    }
}
