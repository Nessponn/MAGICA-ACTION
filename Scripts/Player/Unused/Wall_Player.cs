using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //�G�ɑ΂��铖���蔻��
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //�ǂɐڐG
            PlayerManager.Instance.Wall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //�G�ɑ΂��铖���蔻��
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //�ǂ��痣���
            PlayerManager.Instance.Wall = false;
        }
    }
}
