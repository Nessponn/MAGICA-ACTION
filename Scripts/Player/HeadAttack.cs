using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : MonoBehaviour
{
    //�v���C���[���u���b�N�Ȃǂɑ΂��ē��˂��������ꍇ�̏���

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            if (PlayerManager_Ray.Instance.rbody.velocity.y < 1.5f) return;//��������̑��x�ȏ�Œ@���΍쓮
            //Debug.Log("�R�C���I�I�I�I");
            AudioManager.Instance.Play_SE(5);//���ʉ�

            PlayerManager_Ray.Instance.ForceY(1.5f, 0f);//���ɂ�����Ɛ�����t����
        }
    }
}
