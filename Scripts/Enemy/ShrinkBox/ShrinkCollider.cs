using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkCollider : EnemyManager
{

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//‚Ô‚ÁE‚µ‚½c
    }

    public override void Defeat_Enemy()
    {
        //base.Defeat_Enemy();
        //“¥‚ñ‚¾Œø‰Ê‰¹
        AudioManager.Instance.Play_SE(7);
        ParentObject().SetActive(false);
    }

}
