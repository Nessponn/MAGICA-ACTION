using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //“G‚É‘Î‚·‚é“–‚½‚è”»’è
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //•Ç‚ÉÚG
            PlayerManager.Instance.Wall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //“G‚É‘Î‚·‚é“–‚½‚è”»’è
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Ground")
        {
            //•Ç‚©‚ç—£‚ê‚é
            PlayerManager.Instance.Wall = false;
        }
    }
}
