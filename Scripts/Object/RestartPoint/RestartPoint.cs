using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RestartPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            //Debug.Log("íÜä‘Ç∆Ç¡ÇΩÅI");

            PlayerManager_Ray.Instance.FirstPos = transform.position;

            var obj = transform.GetChild(0).gameObject;

            var obj2 = transform.GetChild(1).gameObject;

            obj.SetActive(true);
            obj2.SetActive(false);

            obj.transform.DOLocalMoveY(1.5f, 0.5f);
        }
    }
}
