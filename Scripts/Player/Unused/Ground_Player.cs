using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[���n�ʂɑ΂��čs�������i��ɃA�j���[�V�����j
public class Ground_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //�G�ɑ΂��铖���蔻��

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //�n�ʂɕt������A�W�����v�\��Ԃɂ���
            //Debug.Log("�W�����v����I�I�I");

            PlayerManager.Instance.Ground();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //�G�ɑ΂��铖���蔻��

        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //�n�ʂɕt������A�W�����v�\��Ԃɂ���
            //Debug.Log("�W�����v�ł��܂ւ�I�I�I");

            PlayerManager.Instance.Jump = false;
            PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.None;
            PlayerManager.Instance.rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
