using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkColliderBody : EnemyManager
{

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//Ç‘Ç¡éEÇµÇΩÅc
    }
    public override void Defeat_Enemy()
    {
        ParentObject().GetComponent<EnemyManager>().Defeat_Enemy();
    }
}
