using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkCollider : EnemyManager
{

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//�Ԃ��E�����c
    }

    public override void Defeat_Enemy()
    {
        //base.Defeat_Enemy();
        //���񂾌��ʉ�
        AudioManager.Instance.Play_SE(7);
        ParentObject().SetActive(false);
    }

}
