using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkColliderBody : EnemyManager
{

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//�Ԃ��E�����c
    }
    public override void Defeat_Enemy()
    {
        ParentObject().GetComponent<EnemyManager>().Defeat_Enemy();
    }
}
