using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class requestLetter : ObjectManager
{
    public GameObject GOTx;


    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            if (ObjectPlayed) return;

            ObjectPlayed = true;

            GetComponent<EffectSystem>().tweenkill();

            if (GOTx) GOTx.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    public override void Object_Reset()
    {
        //base.Object_Reset();

        ObjectPlayed = false;

        if (GOTx) GOTx.SetActive(false);

        if (!gameObject.activeSelf) 
        {
            //Debug.Log("aaaa");
            GetComponent<EffectSystem>().tweenkill();
            GetComponent<EffectSystem>().RestartEffect();
        }

        gameObject.SetActive(true);
    }
}
