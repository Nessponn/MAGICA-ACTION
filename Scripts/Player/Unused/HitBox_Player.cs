using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�ւ̓����蔻��Ɋւ��鏈��
public class HitBox_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //�G�ɑ΂��铖���蔻��

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if(layername == "Enemy")
        {
            //���ɂ܂��I�I�I

            Debug.Log("���񂾁I�I�P");
        }
    }
}
