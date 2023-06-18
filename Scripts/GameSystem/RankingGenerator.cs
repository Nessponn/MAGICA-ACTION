using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingGenerator : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GameMaster.Instance.GetRest());
        }
    }
}
