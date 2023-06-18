using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBarsake : MonoBehaviour
{
    public GameObject BOO;

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            BoomerangBox BB = BOO.GetComponent<BoomerangBox>();

            BB.duration = 0.02f;
            BB.distance = 17f;
            BB.Boomerang_Spped = 2f;
            BB.Boomerang_Height = 20f;
        }
    }
}
