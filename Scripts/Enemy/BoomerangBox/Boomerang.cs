using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    //ブーメランボックスが投げるブーメラン

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            PlayerManager_Ray.Instance.Death_process();//ぶっ殺した…
        }
    }
}
