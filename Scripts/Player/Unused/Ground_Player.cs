using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが地面に対して行う処理（主にアニメーション）
public class Ground_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //敵に対する当たり判定

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //地面に付いたら、ジャンプ可能状態にする
            //Debug.Log("ジャンプしろ！！！");

            PlayerManager.Instance.Ground();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //敵に対する当たり判定

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //地面に付いたら、ジャンプ可能状態にする
            //Debug.Log("ジャンプできまへん！！！");

            PlayerManager.Instance.Jump = false;
            PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.None;
            PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
