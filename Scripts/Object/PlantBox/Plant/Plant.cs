using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plant : ObjectManager
{
    private SpriteRenderer Sp;

    public GameObject PlantObj;
    public int distance;
    private int Count;
    Vector2 pos;

    public override void Start()
    {
        pos = this.transform.position;

        DOVirtual.DelayedCall(0.1f, () =>
        {
            if (Count < distance) Planting();
        });

        Sp = GetComponent<SpriteRenderer>();

        Sp.enabled = false;
    }

    private void Planting()
    {
        Count ++;

        GameObject obj = Instantiate(PlantObj, pos, Quaternion.identity);

        obj.transform.DOLocalMoveY(pos.y + 1, 0.15f);

        pos.y = pos.y + 1;

        DOVirtual.DelayedCall(0.25f, () =>
        {
            if(Count < distance)Planting();
        });
    }

}
